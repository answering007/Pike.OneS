using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pike.OneS.UnitTest.Tests
{
    [TestClass]
    public class OneSConnectorTest
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

        [TestMethod]
        public void ConnectionCanBeOpenedAndClosed()
        {
            OneSConnector dbConnection;
            using (dbConnection = new OneSConnector())
            {
                dbConnection.Connect(_oneSConnectionStringBuilder);
                Assert.IsTrue(dbConnection.IsConnected);
            }
            Assert.IsFalse(dbConnection.IsConnected);
        }
    }
}