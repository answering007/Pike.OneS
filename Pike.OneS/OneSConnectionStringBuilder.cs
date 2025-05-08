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
        static readonly string[] KeyConstants = { nameof(File), nameof(Srvr), nameof(Ref), nameof(Usr), nameof(Pwd) };

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
    }
}
