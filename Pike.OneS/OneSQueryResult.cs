﻿using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace Pike.OneS
{
    /// <summary>
    /// 1C query result
    /// </summary>
    public class OneSQueryResult: OneSBaseComObject
    {
        readonly OneSConnector _connector;

        /// <summary>
        /// Create an instance of <see cref="OneSQueryResult"/>
        /// </summary>
        /// <param name="comQueryResult">Parent COM object (<see cref="OneSBaseComObject.ComObject"/>)</param>
        /// <param name="connector">Associated connector</param>
        internal OneSQueryResult(dynamic comQueryResult, OneSConnector connector)
        {
            if (comQueryResult == null) throw new ArgumentNullException(nameof(comQueryResult));

            ComObject = comQueryResult;
            _connector = connector;
            ParseColumns();
        }

        void ParseColumns()
        {
            var comColumns = ComObject.Columns;
            var columnsCount = (int)comColumns.Count;
            var columns = new OneSQueryResultColumn[columnsCount];

            for (var i = 0; i < columnsCount; i++)
            {
                var comColumn = comColumns.Get(i);
                var comValueType = comColumn.ValueType;
                var columnName = (string) comColumn.Name;

                //Create XmlWriter
                var columnsInfo = (string)_connector.SerializeToXml(comValueType);

                var columnData = XDocument.Parse(columnsInfo);

                var element = columnData.Descendants().FirstOrDefault(n =>
                            n.Name.LocalName.Equals("Type", StringComparison.InvariantCultureIgnoreCase) &&
                            !n.Value.Equals("Null", StringComparison.InvariantCultureIgnoreCase));
                if (element == null)
                {
                    columns[i] = OneSQueryResultColumn.Unknown(columnName, i);
                    continue;
                }

                var stringTypeName = element.Value.ToLowerInvariant().Replace("xs:", string.Empty).Trim();
                if (!KnownTypes.Values.ContainsKey(stringTypeName))
                    stringTypeName = KnownTypes.StringType;
                columns[i] = new OneSQueryResultColumn(columnName, KnownTypes.Values[stringTypeName]) { InternalIndex = i };

                //Release com objects
                Marshal.FinalReleaseComObject(comValueType);
                Marshal.FinalReleaseComObject(comColumn);
            }

            Marshal.FinalReleaseComObject(comColumns);

            Columns = new OneSQueryResultColumnsCollection(columns.Where(col => col.IsDefined));
        }

        /// <summary>
        /// Collection of query result columns
        /// </summary>
        public OneSQueryResultColumnsCollection Columns { get; private set; }

        /// <summary>
        /// Creates a value table and copies all the entries
        /// </summary>
        /// <returns>Value table</returns>
        public OneSValueTable Unload()
        {
            return new OneSValueTable(ComObject.Unload());
        }

        /// <summary>
        /// Unload and creates a <see cref="DataTable"/> from serialized <see cref="OneSValueTable"/>
        /// </summary>
        /// <returns>Table with query result</returns>
        public DataTable DeserializeFromValueTable()
        {
            var dataTable = new DataTable("Result");
            foreach (var column in Columns)
            {
                var name = column.Name;
                var dataColumn = new DataColumn(name)
                {
                    Caption = name,
                    DataType = column.ManagedType,
                    AllowDBNull = true
                };
                dataTable.Columns.Add(dataColumn);
            }

            using (var valueTable = Unload())
            {
                var xmlString = _connector.SerializeToXml(valueTable);
                var data = XDocument.Parse(xmlString);
                var rows =
                    data.Descendants()
                        .Where(n => n.Name.LocalName.Equals("row", StringComparison.InvariantCultureIgnoreCase))
                        .ToArray();

                foreach (var row in rows)
                {
                    var values = row.Elements().ToArray();

                    var dataRow = dataTable.NewRow();
                    for (var i = 0; i < Columns.Count; i++)
                    {
                        var column = Columns[i];
                        if (string.IsNullOrEmpty(values[column.InternalIndex].Value))
                            dataRow[i] = DBNull.Value;
                        else
                        {
                            var type = column.ManagedType;
                            if (type == typeof(DateTime))
                                dataRow[i] = Convert.ToDateTime(values[column.InternalIndex].Value);
                            else if (type == typeof(decimal))
                                dataRow[i] = Convert.ToDecimal(values[column.InternalIndex].Value, CultureInfo.InvariantCulture);
                            else if (type == typeof(bool))
                                dataRow[i] = Convert.ToBoolean(values[column.InternalIndex].Value);
                            else if (type == typeof(string))
                                dataRow[i] = values[column.InternalIndex].Value;
                        }
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }
    }
}
