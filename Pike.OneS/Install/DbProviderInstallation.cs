using Pike.OneS.Data;

namespace Pike.OneS.Install
{
    /// <inheritdoc />
    /// <summary>
    /// 1C .Net provider installation information based on the COM wrapper
    /// </summary>
    public class DbProviderInstallation: DataProviderInstallationBase
    {
        /// <summary>
        /// Create an instance of <see cref="DbProviderInstallation"/>
        /// </summary>
        public DbProviderInstallation() : base(typeof(OneSDbProviderFactory), "1C Data Provider", ".Net Framework Data Provider for 1C 8.x based on the COM wrapper")
        {
        }
    }
}
