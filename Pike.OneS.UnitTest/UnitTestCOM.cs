using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Pike.OneS.UnitTest
{
    [TestClass]
    public class UnitTestCOM
    {
        OneSConnectionStringBuilder _oneSConnectionStringBuilder;

        [TestInitialize]
        public void Init()
        {
            _oneSConnectionStringBuilder = new OneSConnectionStringBuilder
            {
                Database = "Accounting2Server",
                Server = "WIN-U7DLHOF76FQ",
                User = "Абдулов (директор)"
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _oneSConnectionStringBuilder = null;
        }

        [TestMethod]
        public void TestNativeQuery()
        {
            using (var dbConnection = new OneSConnector())
            {
                dbConnection.Connect(_oneSConnectionStringBuilder);
                using (var dbCommand = new OneSQuery(dbConnection))
                {
                    dbCommand.Text = UnitTestHelper.BasicQuery;

                    using (var queryResult = dbCommand.Execute())
                    {
                        var tbl = queryResult.ToDataTable();

                        UnitTestHelper.BasicCompare(tbl);
                    }
                }
            }
        }

        [TestMethod]
        public void TestNativeQueryDeserialize()
        {
            using (var dbConnection = new OneSConnector())
            {
                dbConnection.Connect(_oneSConnectionStringBuilder);
                using (var dbCommand = new OneSQuery(dbConnection))
                {
                    dbCommand.Text = UnitTestHelper.BasicQuery;

                    using (var queryResult = dbCommand.Execute())
                    {
                        var tbl = queryResult.DeserializeFromValueTable();

                        UnitTestHelper.BasicCompare(tbl);
                    }
                }
            }
        }

        [TestMethod]
        public void TestDbStorageStructureInfo()
        {
            using (var connector = new OneSConnector())
            {
                connector.Connect(_oneSConnectionStringBuilder);
                using (var valueTable = connector.GetDbStorageStructureInfo(null, true))
                {
                    var xmlString = connector.SerializeToXml(valueTable);
                    var xdocument = XDocument.Parse(xmlString);
                    
                    Assert.IsNotNull(xdocument);
                }
            }
        }
    }
}
