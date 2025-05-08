using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pike.OneS.UnitTest.Tests.OneS
{
    [TestClass]
    public class OneSConnectorTest
    {
        [TestMethod]
        public void TestConnectionWithLoginAndPassword()
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
        public void TestConnectionWithLoginOnly()
        {
            OneSConnector dbConnection;
            using (dbConnection = new OneSConnector())
            {
                var builder = ConnectionStringBuilder.OneSConnectionStringBuilder;
                builder.Usr = "LoginOnlyUser";
                builder.Pwd = null;
                dbConnection.Connect(builder);
                Assert.IsTrue(dbConnection.IsConnected);
            }
            Assert.IsFalse(dbConnection.IsConnected);
        }

        [TestMethod]
        public void TestConnectionWithWindowsUser()
        {
            OneSConnector dbConnection;
            using (dbConnection = new OneSConnector())
            {
                var builder = ConnectionStringBuilder.OneSConnectionStringBuilder;
                builder.Usr = null;
                builder.Pwd = null;
                dbConnection.Connect(builder);
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