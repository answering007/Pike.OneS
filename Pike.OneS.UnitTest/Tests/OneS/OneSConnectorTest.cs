using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pike.OneS.UnitTest.Tests.OneS
{
    [TestClass]
    public class OneSConnectorTest
    {
        [TestMethod]
        public void ConnectionCanBeOpenedAndClosed()
        {
            OneSConnector dbConnection;
            using (dbConnection = new OneSConnector())
            {
                dbConnection.Connect(ConnectionStringBuilder.OneSConnectionStringBuilder);
                Assert.IsTrue(dbConnection.IsConnected);
            }
            Assert.IsFalse(dbConnection.IsConnected);
        }

        [TestMethod]
        public void TestDbStorageStructureInfo()
        {
            using (var connector = new OneSConnector())
            {
                connector.Connect(ConnectionStringBuilder.OneSConnectionStringBuilder);
                using (var valueTable = connector.GetDbStorageStructureInfo(null, true))
                {
                    var xmlString = connector.SerializeToXml(valueTable);
                    var document = XDocument.Parse(xmlString);

                    Assert.IsNotNull(document);
                }
            }
        }
    }
}