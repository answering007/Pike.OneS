using System;
using System.Collections;
using System.Data.Common;
using System.Linq;

namespace Pike.OneS
{
    /// <summary>
    /// Provides a simple way to create and manage the contents of connection strings used by the <see cref="OneSConnector"/> class
    /// </summary>
    public class OneSConnectionStringBuilder: DbConnectionStringBuilder
    {
        /// <summary>
        /// File key
        /// </summary>
        public const string FileKey = "File";
        /// <summary>
        /// Server key
        /// </summary>
        public const string ServerKey = "Srvr";
        /// <summary>
        /// Database key
        /// </summary>
        public const string DatabaseKey = "Ref";
        /// <summary>
        /// User key
        /// </summary>
        public const string UserKey = "Usr";
        /// <summary>
        /// Password key
        /// </summary>
        public const string PasswordKey = "Pwd";

        readonly string[] _keys = { FileKey, ServerKey, DatabaseKey, UserKey, PasswordKey };

        /// <summary>
        /// Collection of keys
        /// </summary>
        public override ICollection Keys => _keys;

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
                if (!_keys.Contains(keyword))
                    throw new ArgumentException(
                        $"Given keyword is not supported. Supported keyword are: {string.Join(",", _keys)}");
                return base[keyword];
            }
            set
            {
                if (string.IsNullOrWhiteSpace(keyword)) throw new ArgumentException("keyword can't be null or empty");
                if (!_keys.Contains(keyword))
                    throw new ArgumentException(
                        $"Given keyword is not supported. Supported keyword are: {string.Join(",", _keys)}");
                if (value == null) throw new ArgumentNullException(nameof(value));
                base[keyword] = value;
            }
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
    }
}
