using System.Data.Common;

namespace Pike.OneS.Data
{
    /// <summary>
    /// The <see cref="DbProviderFactory"/> implementation
    /// </summary>
    public class OneSDbProviderFactory: DbProviderFactory
    {
        /// <summary>
        /// Instance of <see cref="OneSDbProviderFactory"/>
        /// </summary>
        public static readonly OneSDbProviderFactory Instance = new OneSDbProviderFactory();

        /// <summary>
        /// Specifies whether the specific <see cref="OneSDbProviderFactory"/> supports the <see cref="DbDataSourceEnumerator"/> class. Current value is false
        /// </summary>
        public override bool CanCreateDataSourceEnumerator => false;

        /// <summary>
        /// Returns a new instance of the provider's class that implements the <see cref="OneSDbCommand"/> class
        /// </summary>
        /// <returns>A new instance of <see cref="OneSDbCommand"/></returns>
        public override DbCommand CreateCommand()
        {
            return new OneSDbCommand();
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the <see cref="OneSDbConnection"/> class
        /// </summary>
        /// <returns>A new instance of <see cref="OneSDbConnection"/></returns>
        public override DbConnection CreateConnection()
        {
            return new OneSDbConnection();
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the <see cref="OneSDbConnectionStringBuilder"/> class
        /// </summary>
        /// <returns>A new instance of <see cref="OneSDbConnectionStringBuilder"/></returns>
        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new OneSDbConnectionStringBuilder();
        }

        /// <summary>
        /// Returns a new instance of the provider's class that implements the <see cref="OneSDbParameter"/> class
        /// </summary>
        /// <returns>A new instance of <see cref="OneSDbParameter"/></returns>
        public override DbParameter CreateParameter()
        {
            return new OneSDbParameter();
        }
    }
}
