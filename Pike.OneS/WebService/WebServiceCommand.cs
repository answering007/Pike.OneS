using System;
using System.Data.Common;
using System.Data;

namespace Pike.OneS.WebService
{
    /// <inheritdoc />
    /// <summary>
    /// 1C web service command implementation based on <see cref="T:System.Data.Common.DbCommand" />
    /// </summary>
    public class WebServiceCommand : DbCommand
    {
        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the text command to run against the data source
        /// </summary>
        public override string CommandText { get; set; } = string.Empty;

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the wait time (seconds) before terminating the attempt to execute a command and generating an error. Default value is 60
        /// </summary>
        public override int CommandTimeout { get; set; } = 60;


        /// <inheritdoc />
        /// <summary>
        /// Indicates or specifies how the <see cref="P:Pike.OneS.WebService.WebServiceCommand.CommandText" /> property is interpreted. Only CommandType.Text is supported
        /// </summary>
        public override CommandType CommandType
        {
            get => CommandType.Text;
            set { if (value != CommandType.Text) throw new NotSupportedException(); }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets a value indicating whether the command object should be visible in a customized interface control
        /// </summary>
        public override bool DesignTimeVisible { get; set; } = true;

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets how command results are applied to the <see cref="T:System.Data.DataRow" /> when used by the Update method of a <see cref="T:System.Data.Common.DbDataAdapter" />. Default value is UpdateRowSource.None
        /// </summary>
        public override UpdateRowSource UpdatedRowSource { get; set; } = UpdateRowSource.None;

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the <see cref="T:Pike.OneS.WebService.WebServiceConnection" /> used by command
        /// </summary>
        protected override DbConnection DbConnection { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        protected override DbParameterCollection DbParameterCollection { get; } = null;

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the <see cref="P:Pike.OneS.WebService.WebServiceCommand.DbTransaction" /> within which this <see cref="T:System.Data.Common.DbCommand" /> object executes
        /// </summary>
        protected override DbTransaction DbTransaction { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Creates a prepared (or compiled) version of the command on the data source. Throw new <see cref="NotSupportedException"/>
        /// </summary>
        public override void Prepare()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <summary>
        /// Attempts to cancel the execution. Throw new <see cref="NotSupportedException"/>
        /// </summary>
        public override void Cancel()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <summary>
        /// Create new <see cref="DbParameter"/>. Throw new <see cref="NotSupportedException"/>
        /// </summary>
        /// <returns></returns>
        protected override DbParameter CreateDbParameter()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <summary>
        /// Executes the command text against the connection
        /// </summary>
        /// <param name="behavior">Instance of <see cref="T:System.Data.CommandBehavior" /></param>
        /// <returns>New instance of <see cref="T:System.Data.DataTableReader" /></returns>
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            if (DbConnection == null) throw new InvalidOperationException("DbConnection can't be null");
            if (!(DbConnection is WebServiceConnection connection)) throw new InvalidCastException("Unable to cast DbConnection to the WebServiceConnection");
            if (connection.State != ConnectionState.Open) throw new InvalidOperationException("Connection must be open");
            if (string.IsNullOrWhiteSpace(CommandText)) throw new InvalidOperationException("CommandText can't be null or empty");

            var builder = WebServiceConnectionStringBuilder.Parse(connection.ConnectionString);
            var webService =
                new WebServiceRequest(builder, CommandText) {Timeout = TimeSpan.FromSeconds(CommandTimeout)};
            webService.QueryData();
            
            return webService.ResulTable.CreateDataReader();
        }

        /// <inheritdoc />
        /// <summary>
        /// Executes a statement against a connection object
        /// </summary>
        /// <returns>The number of rows affected</returns>
        public override int ExecuteNonQuery()
        {
            using (var reader = ExecuteReader())
            {
                if (!reader.HasRows) return 0;

                var count = 0;
                while (reader.Read())
                    count++;
                return count;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Executes the query and returns the first column of the first row in the result set returned by the query. All other columns and rows are ignored
        /// </summary>
        /// <returns>The first column of the first row in the result set</returns>
        public override object ExecuteScalar()
        {
            using (var reader = ExecuteReader())
            {
                if (!reader.HasRows) return null;

                reader.Read();
                return reader[0];
            }
        }
    }
}