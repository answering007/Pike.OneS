using System;
using System.Data;
using System.Data.Common;

namespace Pike.OneS.WebService
{
    /// <inheritdoc />
    /// <summary>
    /// 1C web service connection with <see cref="T:System.Data.Common.DbConnection" /> implementation
    /// </summary>
    public class WebServiceConnection: DbConnection
    {
        WebServiceConnectionStringBuilder _builder = new WebServiceConnectionStringBuilder();

        /// <summary>
        /// Get or set 1C web service connection string
        /// </summary>
        public override string ConnectionString
        {
            get { return _builder.ToString(); }
            set { _builder = WebServiceConnectionStringBuilder.Parse(string.IsNullOrWhiteSpace(value) ? string.Empty : value); }
        }

        /// <summary>
        /// Get 1C database name
        /// </summary>
        public override string Database => _builder.Database;

        /// <summary>
        /// Get 1C web service Uri namespace
        /// </summary>
        public override string DataSource => _builder.UriNamespace;

        ConnectionState _state = ConnectionState.Closed;
        /// <inheritdoc />
        /// <summary>
        /// Get the current connection state
        /// </summary>
        public override ConnectionState State => _state;

        /// <inheritdoc />
        /// <summary>
        /// Get server version (return empty string)
        /// </summary>
        public override string ServerVersion => string.Empty;

        /// <inheritdoc />
        /// <summary>
        /// Opens a database connection
        /// </summary>
        public override void Open()
        {
            _state = ConnectionState.Open;
        }

        /// <summary>
        /// Closes the connection to the database
        /// </summary>
        public override void Close()
        {
            _state = ConnectionState.Closed;
        }

        /// <summary>
        /// Creates and returns a <see cref="WebServiceCommand"/> object associated with the current connection
        /// </summary>
        /// <returns>A <see cref="WebServiceCommand"/> object</returns>
        protected override DbCommand CreateDbCommand()
        {
            return new WebServiceCommand {Connection = this, CommandTimeout = _builder.Timeout};
        }

        #region No need to implement

        /// <inheritdoc />
        /// <summary>
        /// Starts a database transaction. Currently throw new <see cref="NotSupportedException"/>
        /// </summary>
        /// <param name="isolationLevel">A <see cref="T:System.Data.IsolationLevel" /> object</param>
        /// <returns></returns>
        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <summary>
        /// Changes the current database for an open connection. Currently throw new <see cref="NotSupportedException"/>
        /// </summary>
        /// <param name="databaseName">Specifies the name of the database for the connection to use</param>
        public override void ChangeDatabase(string databaseName)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}