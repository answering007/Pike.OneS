using System;
using System.Data.Common;
using System.Data;

namespace Pike.OneS.Data
{
    /// <summary>
    /// 1C command implementation based on <see cref="DbCommand"/>
    /// </summary>
    public class OneSDbCommand : DbCommand
    {
        DataTable _queryResultTable;
        
        /// <summary>
        /// Gets or sets the text command to run against the data source
        /// </summary>
        public override string CommandText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error. Default value is 0
        /// </summary>
        public override int CommandTimeout { get; set; } = 0;

        /// <summary>
        /// Indicates or specifies how the <see cref="CommandText"/> property is interpreted. Only CommandType.Text is supported
        /// </summary>
        public override CommandType CommandType
        {
            get => CommandType.Text;
            set { if (value != CommandType.Text) throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the command object should be visible in a customized interface control
        /// </summary>
        public override bool DesignTimeVisible { get; set; } = true;

        /// <summary>
        /// Gets or sets how command results are applied to the <see cref="DataRow"/> when used by the Update method of a <see cref="DbDataAdapter"/>. Default value is UpdateRowSource.None
        /// </summary>
        public override UpdateRowSource UpdatedRowSource { get; set; } = UpdateRowSource.None;

        /// <summary>
        /// Gets or sets the <see cref="OneSDbConnection"/> used by this <see cref="OneSDbCommand"/>
        /// </summary>
        protected override DbConnection DbConnection { get; set; }

        /// <summary>
        /// Gets the collection of <see cref="OneSDbParameter"/> objects
        /// </summary>
        protected override DbParameterCollection DbParameterCollection { get; } = new OneSDbParameterCollection();

        /// <summary>
        /// Gets or sets the <see cref="DbTransaction"/> within which this <see cref="DbCommand"/> object executes
        /// </summary>
        protected override DbTransaction DbTransaction { get; set; }

        /// <summary>
        /// Attempts to cancel the execution of a <see cref="OneSDbCommand"/>. Currently, raise <see cref="NotImplementedException"/>
        /// </summary>
        public override void Cancel()
        {
        }

        DataTable GetQueryResultTable(OneSQueryResult queryResult)
        {
            return _queryResultTable ?? (_queryResultTable = queryResult.DeserializeFromValueTable());
        }

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

        /// <summary>
        /// Creates a prepared (or compiled) version of the command on the data source. Currently not implemented
        /// </summary>
        public override void Prepare()
        {
        }

        /// <summary>
        /// Creates a new instance of a <see cref="OneSDbParameter"/> object.
        /// </summary>
        /// <returns>A <see cref="OneSDbParameter"/> object</returns>
        protected override DbParameter CreateDbParameter()
        {
            return new OneSDbParameter();
        }

        /// <summary>
        /// Executes the command text against the connection
        /// </summary>
        /// <param name="behavior">Instance of <see cref="CommandBehavior"/></param>
        /// <returns>A <see cref="DataTableReader"/></returns>
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            if (DbConnection == null) throw new InvalidOperationException("DbConnection can't be null");
            var connection = (OneSDbConnection)DbConnection;
            if (connection.State != ConnectionState.Open) throw new InvalidOperationException("Connection must be open");

            using (var query = new OneSQuery(connection.OneSConnector))
            {
                query.Text = CommandText;
                var parameters = (OneSDbParameterCollection)DbParameterCollection;
                foreach (var p in parameters.Values)
                    query.SetParameter(p.ParameterName, p.Value);

                DataTable datatable;
                using (var queryResult = query.Execute())
                    datatable = GetQueryResultTable(queryResult);
                return datatable.CreateDataReader();
            }            
        }
    }
}
