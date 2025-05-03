using Pike.OneS.WebService;

namespace Pike.OneS.Install
{
    /// <inheritdoc />
    /// <summary>
    /// 1C .Net provider installation information based on the web service
    /// </summary>
    public class WebServiceProviderInstallation: DataProviderInstallationBase
    {
        /// <summary>
        /// Create an instance of <see cref="WebServiceProviderInstallation"/>
        /// </summary>
        public WebServiceProviderInstallation() : base(typeof(WebServiceDbProviderFactory), "1C Web service data provider", "Ado.Net data provider wrapper for 1C web service")
        {
        }
    }
}
