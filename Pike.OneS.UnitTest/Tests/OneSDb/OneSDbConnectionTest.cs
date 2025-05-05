using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pike.OneS.Data;

namespace Pike.OneS.UnitTest.Tests.OneSDb
{
    [TestClass]
    public class OneSDbConnectionTest
    {
        [TestMethod]
        public void ConnectionCanBeOpenedAndClosed()
        {
            OneSDbConnection dbConnection;
            using (dbConnection = new OneSDbConnection())
            {
                dbConnection.ConnectionString = ConnectionStringBuilder.OneSDbConnectionStringBuilder.ConnectionString;
                dbConnection.Open();
                Assert.IsTrue(dbConnection.State == ConnectionState.Open);
            }
            Assert.IsTrue(dbConnection.State == ConnectionState.Closed);
        }

        [TestMethod]
        public void TestConnectionWithLoginOnly()
        {
            OneSDbConnection dbConnection;
            using (dbConnection = new OneSDbConnection())
            {
                var builder = ConnectionStringBuilder.OneSDbConnectionStringBuilder;
                builder.Usr = "LoginOnlyUser";
                builder.Pwd = null;
                dbConnection.ConnectionString = builder.ToString();
                dbConnection.Open();
                Assert.IsTrue(dbConnection.State == ConnectionState.Open);
            }
            Assert.IsTrue(dbConnection.State == ConnectionState.Closed);
        }

        [TestMethod]
        public void TestConnectionWithWindowsUser()
        {
            OneSDbConnection dbConnection;
            using (dbConnection = new OneSDbConnection())
            {
                var builder = ConnectionStringBuilder.OneSDbConnectionStringBuilder;
                builder.Usr = null;
                builder.Pwd = null;
                dbConnection.ConnectionString = builder.ToString();
                dbConnection.Open();
                Assert.IsTrue(dbConnection.State == ConnectionState.Open);
            }
            Assert.IsTrue(dbConnection.State == ConnectionState.Closed);
        }
    }
}
