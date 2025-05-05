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
        const int DefaultTimeout = 60;

        private static readonly ReadOnlyCollection<string> KeyConstants = new ReadOnlyCollection<string>(new[]
        {
            nameof(Address), nameof(UriNamespace), nameof(Database), nameof(ServiceFileName), nameof(UserName),
            nameof(Password), nameof(Timeout)
        });

        /// <inheritdoc />
        /// <summary>
        /// Collection of keys
        /// </summary>
        public override ICollection Keys => KeyConstants;

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
                if (!KeyConstants.Contains(keyword))
                    throw new ArgumentException(
                        $"Given keyword is not supported. Supported keyword are: {string.Join(",", KeyConstants)}");
                return base[keyword];
            }
            set
            {
                if (!KeyConstants.Contains(keyword))
                    throw new ArgumentException(
                        $"Given keyword is not supported. Supported keyword are: {string.Join(",", KeyConstants)}");
                base[keyword] = value;
            }
        }

        /// <summary>
        /// 1C web service address
        /// </summary>
        public string Address
        {
            get => this[nameof(Address)] as string;
            set => this[nameof(Address)] = value;
        }

        /// <summary>
        /// 1C web service Uri namespace
        /// </summary>
        public string UriNamespace
        {
            get => this[nameof(UriNamespace)] as string;
            set => this[nameof(UriNamespace)] = value;
        }

        /// <summary>
        /// Database name
        /// </summary>
        public string Database
        {
            get => this[nameof(Database)] as string;
            set => this[nameof(Database)] = value;
        }

        /// <summary>
        /// Service file name
        /// </summary>
        public string ServiceFileName
        {
            get => this[nameof(ServiceFileName)] as string;
            set => this[nameof(ServiceFileName)] = value;
        }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName
        {
            get => this[nameof(UserName)] as string;
            set => this[nameof(UserName)] = value;
        }

        /// <summary>
        /// User password
        /// </summary>
        public string Password
        {
            get => this[nameof(Password)] as string;
            set => this[nameof(Password)] = value;
        }

        /// <summary>
        /// Web request timeout in seconds. Default value is 60
        /// </summary>
        public int Timeout
        {
            get
            {
                if (ContainsKey(nameof(Timeout)))
                    return (int)this[nameof(Timeout)];
                return DefaultTimeout;
            }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                this[nameof(Timeout)] = value;
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
            foreach (var key in KeyConstants)
            {
                var startToken = key + "=";
                var kv = values.FirstOrDefault(v => v.StartsWith(startToken, StringComparison.InvariantCultureIgnoreCase));
                if (kv == null) continue;

                var value = kv.Substring(startToken.Length);
                if (key == nameof(Timeout))
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