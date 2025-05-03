using System;
using System.Data.Common;
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;

namespace Pike.OneS.WebService
{
    /// <inheritdoc />
    /// <summary>
    /// Provides a simple way to create and manage the contents of connection strings used by the <see cref="WebServiceConnection"/> class
    /// </summary>
    public class WebServiceConnectionStringBuilder : DbConnectionStringBuilder
    {
        const string AddressKey = "Address";
        const string UriNamespaceKey = "UriNamespace";
        const string DatabaseKey = "Database";
        const string ServiceFileNameKey = "ServiceFileName";
        const string UserNameKey = "UserName";
        const string PasswordKey = "Password";
        const string TimeoutKey = "Timeout";
        const int DefaultTimeout = 60;

        static readonly ReadOnlyCollection<string> KeyConstans = new ReadOnlyCollection<string>(new[]
            {AddressKey, UriNamespaceKey, DatabaseKey, ServiceFileNameKey, UserNameKey, PasswordKey, TimeoutKey});

        /// <inheritdoc />
        /// <summary>
        /// Collection of keys
        /// </summary>
        public override ICollection Keys => KeyConstans;

        /// <inheritdoc />
        /// <summary>
        /// Get the value for the specific key
        /// </summary>
        /// <param name="keyword">Key</param>
        /// <returns>Value for the specific key</returns>
        public override object this[string keyword]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(keyword)) throw new ArgumentException("keyword can't be null or empty");
                if (!KeyConstans.Contains(keyword))
                    throw new ArgumentException(
                        $"Given keyword is not supported. Supported keyword are: {string.Join(",", KeyConstans)}");
                return base[keyword];
            }
            set
            {
                if (string.IsNullOrWhiteSpace(keyword)) throw new ArgumentException("keyword can't be null or empty");
                if (!KeyConstans.Contains(keyword))
                    throw new ArgumentException(
                        $"Given keyword is not supported. Supported keyword are: {string.Join(",", KeyConstans)}");
                base[keyword] = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// 1C web service address
        /// </summary>
        public string Address
        {
            get { return this[AddressKey] as string; }
            set { this[AddressKey] = value; }
        }

        /// <summary>
        /// 1C web service Uri namespace
        /// </summary>
        public string UriNamespace
        {
            get { return this[UriNamespaceKey] as string; }
            set { this[UriNamespaceKey] = value; }
        }

        /// <summary>
        /// Database name
        /// </summary>
        public string Database
        {
            get { return this[DatabaseKey] as string; }
            set { this[DatabaseKey] = value; }
        }

        /// <summary>
        /// Service file name
        /// </summary>
        public string ServiceFileName
        {
            get { return this[ServiceFileNameKey] as string; }
            set { this[ServiceFileNameKey] = value; }
        }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName
        {
            get { return this[UserNameKey] as string; }
            set { this[UserNameKey] = value; }
        }

        /// <summary>
        /// User password
        /// </summary>
        public string Password
        {
            get { return this[PasswordKey] as string; }
            set { this[PasswordKey] = value; }
        }

        /// <summary>
        /// Web request timeout in seconds. Default value is 60
        /// </summary>
        public int Timeout
        {
            get
            {
                if (ContainsKey(TimeoutKey))
                    return (int)this[TimeoutKey];
                return DefaultTimeout;
            }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                this[TimeoutKey] = value;
            }
        }

        /// <summary>
        /// Parse 1C web service connection string
        /// </summary>
        /// <param name="connectionString">Connection string to parse</param>
        /// <returns>New instance of <see cref="WebServiceConnectionStringBuilder"/></returns>
        public static WebServiceConnectionStringBuilder Parse(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException("Value can't be null or empty", nameof(connectionString));

            var rst = new WebServiceConnectionStringBuilder();
            var values = connectionString.
                Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries).
                Select(v => v.Trim()).
                ToArray();
            foreach (var key in KeyConstans)
            {
                var startToken = key + "=";
                var kv = values.FirstOrDefault(v => v.StartsWith(startToken, StringComparison.InvariantCultureIgnoreCase));
                if (kv == null) continue;

                var value = kv.Substring(startToken.Length);
                if (key == TimeoutKey)
                {
                    if (!int.TryParse(value, out var timeout)) continue;
                    if (timeout <= 0) continue;

                    rst[key] = timeout;
                }
                rst[key] = value;
            }

            return rst;
        }

        /// <summary>
        /// Get 1C web service link
        /// </summary>
        public string WebServiceLink
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Address)) return null;
                if (string.IsNullOrWhiteSpace(Database)) return null;
                if (string.IsNullOrWhiteSpace(ServiceFileName)) return null;

                return $@"{Address}/{Database}/ws/{ServiceFileName}";
            }
        }

        /// <summary>
        /// Get 1C web services description metadata
        /// </summary>
        public string WsdlLink => WebServiceLink == null? null: WebServiceLink + "?wsdl";

        /// <summary>
        /// Get 1C web service SOAP action link
        /// </summary>
        public string SoapAction => UriNamespace == null? null: $"{UriNamespace}#WebIntegration:GetQueryResult";
    }
}