using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pike.OneS.WebService;
using System.Data;

namespace Pike.OneS.UnitTest.Tests
{
    [TestClass]
    public class WebServiceTest
    {
        [TestMethod]
        public void TestWebServiceWithLoginAndPassword()
        {
            var serviceRequest = new WebServiceRequest(ConnectionStringBuilder.WebServiceConnectionStringBuilder, TestHelper.BasicQuery);
            serviceRequest.QueryData();
            TestHelper.BasicCompare(serviceRequest.ResulTable);
        }

        [TestMethod]
        public void TestWebServiceWithLoginOnly()
        {
            var builder = ConnectionStringBuilder.WebServiceConnectionStringBuilder;
            builder.UserName = "LoginOnlyUser";
            builder.Password = null;
            var serviceRequest = new WebServiceRequest(builder, TestHelper.BasicQuery);
            serviceRequest.QueryData();
            TestHelper.BasicCompare(serviceRequest.ResulTable);
        }

        [TestMethod]
        public void TestOneSDbCommandResult()
        {
            using (var dbConnection = new WebServiceConnection())
            {
                dbConnection.ConnectionString = ConnectionStringBuilder.WebServiceConnectionStringBuilder.ConnectionString;
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.Connection = dbConnection;
                    dbCommand.CommandText = TestHelper.BasicQuery;
                    var result = new DataTable();
                    using (var dbReader = dbCommand.ExecuteReader())
                    {
                        result.Load(dbReader);
                        TestHelper.BasicCompare(result);
                    }
                }
            }
        }
    }
}
