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
                Database = SettingsConnection.Default.Database,
                Server = SettingsConnection.Default.Server,
                User = SettingsConnection.Default.User
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
                    dbCommand.Text = TestHelper.BasicQuery;

                    using (var queryResult = dbCommand.Execute())
                    {
                        var tbl = queryResult.ToDataTable();

                        TestHelper.BasicCompare(tbl);
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
                    dbCommand.Text = TestHelper.BasicQuery;

                    using (var queryResult = dbCommand.Execute())
                    {
                        var tbl = queryResult.DeserializeFromValueTable();

                        TestHelper.BasicCompare(tbl);
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
