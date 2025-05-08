using System.Data.Common;

namespace Pike.ConnectionTest.ConnectionInfo
{
    public interface IConnectionInfo
    {
        string Name { get; }
        
        char Id { get; }

        DbConnectionStringBuilder Builder { get; }

        void TestConnection();
    }
}