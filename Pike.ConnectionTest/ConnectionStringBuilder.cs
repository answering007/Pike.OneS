using Pike.OneS.Data;
using Pike.OneS.WebService;

namespace Pike.ConnectionTest
{
    internal class ConnectionStringBuilder
    {
        public static OneSDbConnectionStringBuilder OneSDbConnectionStringBuilder =>
            new OneSDbConnectionStringBuilder
            {
                ProgId = SettingsConnection.Default.ProgId,
                Database = SettingsConnection.Default.Database,
                Server = SettingsConnection.Default.Server,
                User = SettingsConnection.Default.User
            };

        public static WebServiceConnectionStringBuilder WebServiceConnectionStringBuilder =>
            new WebServiceConnectionStringBuilder
            {
                Address = SettingsWebService.Default.Address,
                UriNamespace = SettingsWebService.Default.UriNamespace,
                Database = SettingsWebService.Default.Database,
                ServiceFileName = SettingsWebService.Default.ServiceFileName,
                UserName = SettingsWebService.Default.UserName,
                Password = SettingsWebService.Default.Password
            };
    }
}
