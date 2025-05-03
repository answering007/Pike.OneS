using System.Data.Common;

namespace Pike.OneS.WebService
{
    /// <inheritdoc />
    /// <summary>
    /// 1C web service <see cref="T:Pike.OneS.WebService.WebServiceDbProviderFactory" /> implementation
    /// </summary>
    public class WebServiceDbProviderFactory: DbProviderFactory
    {
        /// <summary>
        /// Instance of <see cref="WebServiceDbProviderFactory"/>
        /// </summary>
        public static readonly WebServiceDbProviderFactory Instance = new WebServiceDbProviderFactory();

        /// <inheritdoc />
        /// <summary>
        /// Specifies whether the specific <see cref="T:Pike.OneS.WebService.WebServiceDbProviderFactory" /> supports the <see cref="T:System.Data.Common.DbDataSourceEnumerator" /> class. Current value is false
        /// </summary>
        public override bool CanCreateDataSourceEnumerator => false;

        /// <inheritdoc />
        /// <summary>
        /// Returns a new instance of the provider's class that implements the <see cref="T:Pike.OneS.WebService.WebServiceCommand" /> class
        /// </summary>
        /// <returns>A new instance of <see cref="T:Pike.OneS.WebService.WebServiceCommand" /></returns>
        public override DbCommand CreateCommand()
        {
            return new WebServiceCommand();
        }

        /// <inheritdoc />
        /// <summary>
        /// Returns a new instance of the provider's class that implements the <see cref="T:Pike.OneS.WebService.WebServiceConnection" /> class
        /// </summary>
        /// <returns>A new instance of <see cref="T:Pike.OneS.WebService.WebServiceConnection" /></returns>
        public override DbConnection CreateConnection()
        {
            return new WebServiceConnection();
        }

        /// <inheritdoc />
        /// <summary>
        /// Returns a new instance of the provider's class that implements the <see cref="T:Pike.OneS.WebService.WebServiceConnectionStringBuilder" /> class
        /// </summary>
        /// <returns>A new instance of <see cref="T:Pike.OneS.WebService.WebServiceConnectionStringBuilder" /></returns>
        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new WebServiceConnectionStringBuilder();
        }
    }
}
