using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pike.OneS.Data;
using System.Data;

namespace Pike.OneS.UnitTest.Tests.OneSDb
{
    [TestClass]
    public class OneSDbCommandTest
    {
        [TestMethod]
        public void TestOneSDbCommandResult()
        {
            using (var dbConnection = new OneSDbConnection())
            {
                dbConnection.ConnectionString = ConnectionStringBuilder.OneSDbConnectionStringBuilder.ConnectionString;
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
