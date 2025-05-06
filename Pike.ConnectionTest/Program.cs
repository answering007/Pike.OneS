using System;
using System.Collections.Generic;
using System.Linq;
using Pike.ConnectionTest.ConnectionInfo;
using Pike.OneS.Data;
using Pike.OneS.WebService;

namespace Pike.ConnectionTest
{
    internal class Program
    {
        private static IDictionary<char, IConnectionInfo> Connections { get; set; }

        const string Separator = "=======================";

        static void Main()
        {
            Connections = new IConnectionInfo[]
            {
                new FactoryConnectionInfo("ADO.Net COM connection", '0',
                    ConnectionStringBuilder.OneSDbConnectionStringBuilder, typeof(OneSDbProviderFactory).FullName),
                new FactoryConnectionInfo("ADO.Net Web service connection", '1',
                    ConnectionStringBuilder.WebServiceConnectionStringBuilder,
                    typeof(WebServiceDbProviderFactory).FullName),
                new WebServiceConnectionInfo("Web service connection", '2')
            }.ToDictionary(kv => kv.Id);
            
            Console.WriteLine("Test connection:");
            TestConnections();
            Console.WriteLine(Separator);
            Console.WriteLine("Done!");
        }

        static void TestConnections()
        {
            while (true)
            {
                Console.WriteLine(Separator);
                Console.WriteLine("Choose connection:");
                foreach (var item in Connections.Values)
                    Console.WriteLine($"[{item.Id}]: for {item.Name}");


                var symbol = Console.ReadKey();
                Console.WriteLine("");
                if (!Connections.TryGetValue(symbol.KeyChar, out var connectionInfo)) break;

                Console.WriteLine(Separator);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(connectionInfo.Name);
                Console.ResetColor();
                Console.WriteLine($"ConnectionString: {connectionInfo.Builder}");

                connectionInfo.TestConnection();
            }
        }
    }
}
