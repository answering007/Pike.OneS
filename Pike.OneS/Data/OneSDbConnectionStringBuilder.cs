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
        const string ProgIdKey = "ProgId";
        const string FileKey = OneSConnectionStringBuilder.FileKey;
        const string ServerKey = OneSConnectionStringBuilder.ServerKey;
        const string DatabaseKey = OneSConnectionStringBuilder.DatabaseKey;
        const string UserKey = OneSConnectionStringBuilder.UserKey;
        const string PasswordKey = OneSConnectionStringBuilder.PasswordKey;

        static readonly string[] KeyConstants = { ProgIdKey, FileKey, ServerKey, DatabaseKey, UserKey, PasswordKey };

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
                if (string.IsNullOrWhiteSpace(keyword)) throw new ArgumentException("keyword can't be null or empty");
                if (!KeyConstants.Contains(keyword))
                    throw new ArgumentException(
                        $"Given keyword is not supported. Supported keyword are: {string.Join(",", KeyConstants)}");
                return base[keyword];
            }
            set
            {
                if (string.IsNullOrWhiteSpace(keyword)) throw new ArgumentException("keyword can't be null or empty");
                if (!KeyConstants.Contains(keyword))
                    throw new ArgumentException(
                        $"Given keyword is not supported. Supported keyword are: {string.Join(",", KeyConstants)}");
                if (value == null) throw new ArgumentNullException(nameof(value));
                base[keyword] = value;
            }
        }

        /// <summary>
        /// 1C COM program identifier. Example: V83.ComConnector
        /// </summary>
        public string ProgId
        {
            get => this[ProgIdKey] as string;
            set => this[ProgIdKey] = value;
        }

        /// <summary>
        /// Path to the folder for file-server connection type
        /// </summary>
        public string File
        {
            get => this[FileKey] as string;
            set => this[FileKey] = value;
        }

        /// <summary>
        /// Server name for client-server connection type
        /// </summary>
        public string Server
        {
            get => this[ServerKey] as string;
            set => this[ServerKey] = value;
        }

        /// <summary>
        /// Database name for client-server connection type
        /// </summary>
        public string Database
        {
            get => this[DatabaseKey] as string;
            set => this[DatabaseKey] = value;
        }

        /// <summary>
        /// User name
        /// </summary>
        public string User
        {
            get => this[UserKey] as string;
            set => this[UserKey] = value;
        }

        /// <summary>
        /// Password
        /// </summary>
        public string Password
        {
            get => this[PasswordKey] as string;
            set => this[PasswordKey] = value;
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

            var keys = KeyConstants.Where(k => !k.Equals(ProgIdKey, StringComparison.InvariantCultureIgnoreCase)).ToArray();
            foreach (var key in keys)
            {
                if (!ContainsKey(key)) continue;
                rst[key] = this[key];
            }

            return rst;
        }
    }
}
