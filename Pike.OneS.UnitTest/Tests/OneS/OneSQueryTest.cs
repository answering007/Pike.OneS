using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pike.OneS.UnitTest.Tests.OneS
{
    [TestClass]
    public class OneSQueryTest
    {
        [TestMethod]
        public void TestOneSQueryResultDeserializeFromValueTable()
        {
            using (var dbConnection = new OneSConnector())
            {
                dbConnection.Connect(ConnectionStringBuilder.OneSConnectionStringBuilder);
                using (var dbCommand = new OneSQuery(dbConnection))
                {
                    dbCommand.Text = TestHelper.BasicQuery;

                    using (var queryResult = dbCommand.Execute())
                    {
                        var tbl = queryResult.DeserializeFromValueTable();
                        TestHelper.BasicCompare(tbl);
                    }
                }
            }
        }
    }
}
