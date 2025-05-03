using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pike.OneS.WebService;
using System.Data.Common;

namespace Pike.OneS.UnitTest
{
    [TestClass]
    public class UnitTestWebService
    {
        WebServiceConnectionStringBuilder _webServiceBuilder;

        [TestInitialize]
        public void Init()
        {
            _webServiceBuilder = new WebServiceConnectionStringBuilder
            {
                Address = "http://192.168.189.128",
                UriNamespace = "http://10.10.15.150/WebIntegration",
                Database = "Accounting2Server",
                ServiceFileName = "WebIntegration.1cws",
                UserName = "Integration",
                Password = "Integration123"
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _webServiceBuilder = null;
        }

        [TestMethod]
        public void TestNativeWebService()
        {
            var serviceRequest = new WebServiceRequest(_webServiceBuilder, UnitTestHelper.BasicQuery);
            serviceRequest.QueryData();
            UnitTestHelper.
                        BasicCompare(serviceRequest.ResulTable);
        }

        [TestMethod]
        public void TestWebServiceProviderFactory()
        {
            var factory = DbProviderFactories.GetFactory(typeof(WebServiceDbProviderFactory).FullName);

            var data = UnitTestHelper.GetData(factory, _webServiceBuilder, UnitTestHelper.BasicQuery);
            UnitTestHelper.BasicCompare(data);
        }
    }
}
