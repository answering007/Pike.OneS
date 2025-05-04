using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pike.OneS.Data;
using Pike.OneS.WebService;

namespace Pike.OneS.UnitTest.Tests
{
    [TestClass]
    public class DbProviderFactoryTest
    {
        static void TestProviderFactory(string factoryName, DbConnectionStringBuilder builder)
        {
            var factory = DbProviderFactories.GetFactory(factoryName);
            var data = TestHelper.GetData(factory, builder, TestHelper.BasicQuery);
            TestHelper.BasicCompare(data);
        }

        [TestMethod]
        public void TestOneSDbProviderFactory()
        {
            TestProviderFactory(typeof(OneSDbProviderFactory).FullName, ConnectionStringBuilder.OneSDbConnectionStringBuilder);
        }

        [TestMethod]
        public void TestWebServiceProviderFactory()
        {
            TestProviderFactory(typeof(WebServiceDbProviderFactory).FullName, ConnectionStringBuilder.WebServiceConnectionStringBuilder);
        }
    }
}
