using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pike.OneS.WebService;
using System.Data.Common;

namespace Pike.OneS.UnitTest
{
    [TestClass]
    public class WebServiceTest
    {
        WebServiceConnectionStringBuilder _webServiceBuilder;

        [TestInitialize]
        public void Init()
        {
            var secret = TestHelper.LoadSecret<Secret>(SettingsMain.Default.SecretPath);
            _webServiceBuilder = new WebServiceConnectionStringBuilder
            {
                Address = "http://192.168.189.129",
                UriNamespace = "http://10.10.15.150/WebIntegration",
                Database = "AccountingServer",
                ServiceFileName = "WebIntegration.1cws",
                UserName = secret.UserName,
                Password = secret.Password
            };
        }

        [TestMethod]
        public void TestNativeWebService()
        {
            var serviceRequest = new WebServiceRequest(_webServiceBuilder, TestHelper.BasicQuery);
            serviceRequest.QueryData();
            TestHelper.BasicCompare(serviceRequest.ResulTable);
        }

        [TestMethod]
        public void TestWebServiceProviderFactory()
        {
            var providerInvariantName = typeof(WebServiceDbProviderFactory).FullName;
            if (providerInvariantName == null) throw new Exception("Factory provider not found");

            var factory = DbProviderFactories.GetFactory(providerInvariantName);
            var data = TestHelper.GetData(factory, _webServiceBuilder, TestHelper.BasicQuery);
            TestHelper.BasicCompare(data);
        }
    }
}
