using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pike.OneS.WebService;

namespace Pike.OneS.UnitTest.Tests
{
    [TestClass]
    public class WebServiceTest
    {
        WebServiceConnectionStringBuilder _webServiceBuilder;

        [TestInitialize]
        public void Init()
        {
            _webServiceBuilder = new WebServiceConnectionStringBuilder
            {
                Address = SettingsWebService.Default.Address,
                UriNamespace = SettingsWebService.Default.UriNamespace,
                Database = SettingsWebService.Default.Database,
                ServiceFileName = SettingsWebService.Default.ServiceFileName,
                UserName = SettingsWebService.Default.UserName,
                Password = SettingsWebService.Default.Password
            };
        }

        [TestMethod]
        public void TestNativeWebService()
        {
            var serviceRequest = new WebServiceRequest(_webServiceBuilder, TestHelper.BasicQuery);
            serviceRequest.QueryData();
            TestHelper.BasicCompare(serviceRequest.ResulTable);
        }
    }
}
