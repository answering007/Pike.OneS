using System;
using System.Data.Common;
using System.Linq;
using System.Collections;

namespace Pike.OneS.Data
{
    /// <summary>
    /// Provides a simple way to create and manage the contents of connection strings used by the <see cref="OneSDbConnection"/> class
    /// </summary>
    public class OneSDbConnectionStringBuilder: DbConnectionStringBuilder
    {
        static readonly string[] KeyConstants =
            { nameof(ProgId), nameof(File), nameof(Srvr), nameof(Ref), nameof(Usr), nameof(Pwd) };

        /// <summary>
        /// Collection of keys
        /// </summary>
        public override ICollection Keys => KeyConstants;

        /// <summary>
        /// Get the value for the specific key
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
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
        /// 1C COM program identifier. Example: V83.ComConnector
        /// </summary>
        public string ProgId
        {
            get => this[nameof(ProgId)] as string;
            set => this[nameof(ProgId)] = value;
        }

        /// <summary>
        /// Path to the folder for file-server connection type
        /// </summary>
        public string File
        {
            get => this[nameof(File)] as string;
            set => this[nameof(File)] = value;
        }

        /// <summary>
        /// Server name for client-server connection type
        /// </summary>
        public string Srvr
        {
            get => this[nameof(Srvr)] as string;
            set => this[nameof(Srvr)] = value;
        }

        /// <summary>
        /// Database name for client-server connection type
        /// </summary>
        public string Ref
        {
            get => this[nameof(Ref)] as string;
            set => this[nameof(Ref)] = value;
        }

        /// <summary>
        /// User name
        /// </summary>
        public string Usr
        {
            get => this[nameof(Usr)] as string;
            set => this[nameof(Usr)] = value;
        }

        /// <summary>
        /// Password
        /// </summary>
        public string Pwd
        {
            get => this[nameof(Pwd)] as string;
            set => this[nameof(Pwd)] = value;
        }

        /// <summary>
        /// Parse 1C connection string
        /// </summary>
        /// <param name="connectionString">Connection string to parse</param>
        /// <returns>New instance of <see cref="OneSDbConnectionStringBuilder"/></returns>
        public static OneSDbConnectionStringBuilder Parse(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException("Value can't be null or empty", nameof(connectionString));

            var rst = new OneSDbConnectionStringBuilder();
            var values = connectionString.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var key in KeyConstants)
            {
                var startToken = key + "=";
                var kv = values.FirstOrDefault(v => v.StartsWith(startToken, StringComparison.InvariantCultureIgnoreCase));
                if (kv == null) continue;

                var value = kv.Substring(startToken.Length);
                value = ((value.Length >= 1) && (char.IsPunctuation(value[0]))) ? value.Substring(1) : value;
                value = ((value.Length >= 1) && (char.IsPunctuation(value[value.Length - 1]))) ? value.Remove(value.Length - 1) : value;
                rst[key] = value;
            }

            return rst;
        }

        /// <summary>
        /// Create an instance of <see cref="OneS.OneSConnectionStringBuilder"/> based on current instance values
        /// </summary>
        /// <returns></returns>
        public OneSConnectionStringBuilder GetNativeBuilder()
        {
            var rst = new OneSConnectionStringBuilder();

            var keys = KeyConstants.Where(k => !k.Equals(nameof(ProgId), StringComparison.InvariantCultureIgnoreCase)).ToArray();
            foreach (var key in keys)
            {
                if (!ContainsKey(key)) continue;
                rst[key] = this[key];
            }

            return rst;
        }
    }
}
