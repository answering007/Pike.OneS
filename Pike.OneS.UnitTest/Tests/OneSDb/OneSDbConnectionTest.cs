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
    }
}
