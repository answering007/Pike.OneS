using System;

namespace Pike.OneS
{
    /// <summary>
    /// 1C query
    /// </summary>
    public class OneSQuery: OneSBaseComObject
    {
        const string QueryObject = "Запрос";
        readonly OneSConnector _connector;
        
        /// <summary>
        /// Create an instance of <see cref="OneSQuery"/>
        /// </summary>
        /// <param name="connector">Associated connector. Should be connected before query creation</param>
        public OneSQuery(OneSConnector connector)
        {
            if (connector == null) throw new ArgumentNullException(nameof(connector));
            if (!connector.IsConnected) throw new ArgumentException("connector should be connected");

            _connector = connector;
            ComObject = _connector.ComObject.NewObject(QueryObject);
        }

        /// <summary>
        /// Query text
        /// </summary>
        public string Text
        {
            get => ComObject.Text;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("value can't be null or empty");

                ComObject.Text = value;
            }
        }

        /// <summary>
        /// Set parameters for the query. Parameters should be specified in the query
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="value">Parameter value</param>
        public void SetParameter(string parameterName, object value)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentException("parameterName can't be null or empty");

            ComObject.SetParameter(parameterName, value);
        }

        /// <summary>
        /// Execute the query
        /// </summary>
        /// <returns></returns>
        public OneSQueryResult Execute()
        {
            if (string.IsNullOrWhiteSpace(Text)) throw new Exception("Query text can't be null or empty");

            return new OneSQueryResult(ComObject.Execute(), _connector);
        }
    }
}
