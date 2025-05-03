using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Pike.OneS.WebService
{
    /// <summary>
    /// Web request to query data from 1C web service
    /// </summary>
    public class WebServiceRequest
    {
        const string ContentPattern = @"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
	<soap:Body>
		<m:GetQueryResult xmlns:m=""{0}"">
			<m:QueryText xmlns:xs=""http://www.w3.org/2001/XMLSchema""
					xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">{1}</m:QueryText>
		</m:GetQueryResult>
	</soap:Body>
</soap:Envelope>";

        XDocument _valueTable;

        /// <summary>
        /// Query result data table
        /// </summary>
        public DataTable ResulTable { get; } = new DataTable("Result");

        /// <summary>
        /// Connection string builder to use
        /// </summary>
        public WebServiceConnectionStringBuilder ConnectionStringBuilder { get; }

        /// <summary>
        /// Query to execute
        /// </summary>
        public string Query { get; }

        /// <summary>
        /// Web request content
        /// </summary>
        public byte[] Content { get; }

        /// <summary>
        /// Encoding to use
        /// </summary>
        public Encoding Utf8 => Encoding.UTF8;

        /// <summary>
        /// Web request timeout (round to milliseconds). Default value is 60 seconds
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(60);

        string Base64Encode(string plainText)
        {
            var plainTextBytes = Utf8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        WebRequest CreateRequest()
        {
            var credentials = Base64Encode(string.Join(":", ConnectionStringBuilder.UserName, ConnectionStringBuilder.Password));

            var request = WebRequest.Create(ConnectionStringBuilder.WebServiceLink);
            request.Timeout = (int)Timeout.TotalMilliseconds;
            request.Method = "POST";
            request.ContentType = @"text/xml;charset=""utf-8""";
            request.ContentLength = Content.LongLength;
            request.Headers["Authorization"] = $"Basic {credentials}";
            request.Headers["SOAPAction"] = ConnectionStringBuilder.SoapAction;

            return request;
        }

        void FillResponce(WebRequest request)
        {
            //Write content to then request stream
            using (var dataStream = request.GetRequestStream())
                dataStream.Write(Content, 0, Content.Length);

            //Get the response
            using (var response = request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream == null) return;

                    using (var reader = new StreamReader(responseStream, Utf8))
                    {
                        var xmlString = reader.ReadToEnd();
                        var xmlResponse = XDocument.Parse(xmlString);

                        if (xmlResponse.Root == null) throw new InvalidOperationException("Response root element is null");
                        var returnNode = xmlResponse.Descendants().FirstOrDefault(n =>
                            n.Name.LocalName.Equals("return", StringComparison.InvariantCultureIgnoreCase));
                        if (returnNode == null) throw new InvalidOperationException("Response return node is null");
                        //_valueTable = XDocument.Parse(xmlResponse.Root.Value);
                        _valueTable = XDocument.Parse(returnNode.Value);
                    }
                }
            }
        }

        DataColumn[] ParseRawColumns()
        {
            //Parse columns
            var columns =
                _valueTable.Descendants()
                    .Where(n => n.Name.LocalName.Equals("column", StringComparison.InvariantCultureIgnoreCase))
                    .ToArray();
            
            var rawColumns = new DataColumn[columns.Length];
            for (var i = 0; i < columns.Length; i++)
            {
                //Get columns data and ignore non-primitive data types
                var column = columns[i];
                var columnName = column.FirstNode as XElement;
                if (columnName == null) continue;

                var columnValueType = column.Descendants()
                    .FirstOrDefault(n => n.Name.LocalName.Equals("ValueType", StringComparison.InvariantCultureIgnoreCase));

                var valueType = columnValueType?.Descendants()
                    .FirstOrDefault(n => 
                        n.Name.LocalName.Equals("Type", StringComparison.InvariantCultureIgnoreCase) &&
                        !n.Value.Equals("Null", StringComparison.InvariantCultureIgnoreCase));
                if (string.IsNullOrWhiteSpace(valueType?.Value)) continue;

                var stringType = valueType.Value.Replace("xs:", string.Empty).Trim().ToLowerInvariant();
                if (!KnownTypes.Values.ContainsKey(stringType)) continue;

                //Create column only if it has a primitive data type
                var dataColumn = new DataColumn(columnName.Value)
                {
                    Caption = columnName.Value,
                    DataType = KnownTypes.Values[stringType],
                    AllowDBNull = true
                };

                //Store it
                rawColumns[i] = dataColumn;
            }

            return rawColumns;
        }

        void ResetTable(DataColumn[] rawColumns)
        {
            //Clear table and set new columns
            ResulTable.BeginInit();
            ResulTable.Clear();
            ResulTable.Columns.Clear();
            var existedColumns = rawColumns.Where(kv => kv != null).ToArray();
            ResulTable.Columns.AddRange(existedColumns);
            ResulTable.EndInit();
        }

        void FillDataTable(DataColumn[] columns)
        {
            var rows =
                _valueTable.Descendants()
                    .Where(n => n.Name.LocalName.Equals("row", StringComparison.InvariantCultureIgnoreCase))
                    .ToArray();

            ResulTable.BeginLoadData();
            foreach (var row in rows)
            {
                var values = row.Elements().ToArray();
                var dataRow = ResulTable.NewRow();

                for (var i = 0; i < columns.Length; i++)
                {
                    var column = columns[i];
                    if (column == null) continue;

                    if (string.IsNullOrEmpty(values[i].Value))
                        dataRow[column.ColumnName] = DBNull.Value;
                    else
                    {
                        var type = column.DataType;
                        if (type == typeof(DateTime))
                            dataRow[column.ColumnName] = Convert.ToDateTime(values[i].Value);
                        else if (type == typeof(decimal))
                            dataRow[column.ColumnName] = Convert.ToDecimal(values[i].Value, CultureInfo.InvariantCulture);
                        else if (type == typeof(bool))
                            dataRow[column.ColumnName] = Convert.ToBoolean(values[i].Value);
                        else if (type == typeof(string))
                            dataRow[column.ColumnName] = values[i].Value;
                    }
                }
                ResulTable.Rows.Add(dataRow);
            }
            ResulTable.EndLoadData();
        }

        /// <summary>
        /// Create an instance of <see cref="WebServiceRequest"/>
        /// </summary>
        /// <param name="builder">Connection string builder to use</param>
        /// <param name="query">Query to execute</param>
        public WebServiceRequest(WebServiceConnectionStringBuilder builder, string query)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(query)) throw new ArgumentException("query can't be null or empty");

            ConnectionStringBuilder = builder;
            Query = query;

            var content = string.Format(ContentPattern, ConnectionStringBuilder.UriNamespace, Query);
            Content = Utf8.GetBytes(content);
        }

        /// <summary>
        /// Query data from 1C web service
        /// </summary>
        public void QueryData()
        {
            var request = CreateRequest();
            FillResponce(request);
            var rawColumns = ParseRawColumns();
            ResetTable(rawColumns);
            FillDataTable(rawColumns);
        }
    }
}