using System;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pike.OneS.WebService;

namespace Pike.OneS.UnitTest.Tests
{
    [TestClass]
    public class DbProviderFactoryTest
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
