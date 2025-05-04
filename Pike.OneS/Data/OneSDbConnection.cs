using System;
using System.Data.Common;
using System.Data;

namespace Pike.OneS.Data
{
    /// <inheritdoc />
    /// <summary>
    /// 1C connection with <see cref="T:System.Data.Common.DbConnection" /> implementation
    /// </summary>
    public class OneSDbConnection : DbConnection
    {
        OneSDbConnectionStringBuilder _builder = new OneSDbConnectionStringBuilder();

        /// <summary>
        /// Native <see cref="OneSConnector"/> connector
        /// </summary>
        public OneSConnector OneSConnector { get; private set; }

        /// <summary>
        /// Get or set 1C connection string, including Com-object ProgId
        /// </summary>
        public override string ConnectionString
        {
            get => _builder.ToString();
            set => _builder = OneSDbConnectionStringBuilder.Parse(string.IsNullOrWhiteSpace(value) ? string.Empty : value);
        }

        /// <summary>
        /// Get the database name for client-server connection
        /// </summary>
        public override string Database => string.IsNullOrWhiteSpace(_builder.Database)? string.Empty: _builder.Database;

        /// <summary>
        /// Get the data source (server name for client-server connection; file path for file-server connection)
        /// </summary>
        public override string DataSource
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_builder.Server)) return _builder.Server;
                return !string.IsNullOrWhiteSpace(_builder.File) ? _builder.File : string.Empty;
            }
        }

        /// <summary>
        /// Get the 1C Com-object program identifier
        /// </summary>
        public override string ServerVersion => string.IsNullOrWhiteSpace(_builder.ProgId)? string.Empty: _builder.ProgId;

        ConnectionState _state = ConnectionState.Closed;
        /// <summary>
        /// Get the current connection state
        /// </summary>
        public override ConnectionState State => _state;

        /// <summary>
        /// Opens a database connection with the settings specified by the <see cref="ConnectionString"/>
        /// </summary>
        public override void Open()
        {
            try
            {
                _state = ConnectionState.Connecting;
                OneSConnector = new OneSConnector(_builder.ProgId);
                OneSConnector.Connect(_builder.GetNativeBuilder());
                _state = ConnectionState.Open;
            }
            catch(Exception exception)
            {
                _state = ConnectionState.Broken;
                throw new Exception($"Unable to open connection to [{_builder}]", exception);                
            }
            
        }

        /// <summary>
        /// Closes the connection to the database
        /// </summary>
        public override void Close()
        {
            if (OneSConnector == null) return;

            OneSConnector.Dispose();
            OneSConnector = null;
            _state = ConnectionState.Closed;
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            Close();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates and returns a <see cref="OneSDbCommand"/> object associated with the current connection
        /// </summary>
        /// <returns>A <see cref="OneSDbCommand"/> object</returns>
        protected override DbCommand CreateDbCommand()
        {
            return new OneSDbCommand { Connection = this };
        }

        #region No need to implement

        /// <summary>
        /// Starts a database transaction. Currently, throw <see cref="NotImplementedException"/>
        /// </summary>
        /// <param name="isolationLevel">A <see cref="IsolationLevel"/> object</param>
        /// <returns></returns>
        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Changes the current database for an open connection. Currently, throw <see cref="NotImplementedException"/>
        /// </summary>
        /// <param name="databaseName">Specifies the name of the database for the connection to use</param>
        public override void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
