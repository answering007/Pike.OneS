using Pike.OneS.Data;
using Pike.OneS.WebService;

namespace Pike.OneS.UnitTest
{
    internal class ConnectionStringBuilder
    {
        public static OneSConnectionStringBuilder OneSConnectionStringBuilder =>
            new OneSConnectionStringBuilder
            {
                Ref = SettingsConnection.Default.Database,
                Srvr = SettingsConnection.Default.Server,
                Usr = SettingsConnection.Default.User,
                Pwd = SettingsConnection.Default.Password
            };

        public static OneSDbConnectionStringBuilder OneSDbConnectionStringBuilder =>
            new OneSDbConnectionStringBuilder
            {
                ProgId = SettingsConnection.Default.ProgId,
                Ref = SettingsConnection.Default.Database,
                Srvr = SettingsConnection.Default.Server,
                Usr = SettingsConnection.Default.User,
                Pwd = SettingsConnection.Default.Password
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
