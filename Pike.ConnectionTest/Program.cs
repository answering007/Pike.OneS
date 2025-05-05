using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using Pike.OneS.Data;
using Pike.OneS.WebService;

namespace Pike.ConnectionTest
{
    internal class Program
    {
        public const string BasicQuery = @"ВЫБРАТЬ 1, ДАТАВРЕМЯ(2018, 03, 15, 1, 2, 3), ""Привет"", ИСТИНА";

        private static readonly IDictionary<char, Tuple<DbConnectionStringBuilder, string>> Builders =
            new Dictionary<char, Tuple<DbConnectionStringBuilder, string>>
            {
                {
                    '0',
                    new Tuple<DbConnectionStringBuilder, string>(ConnectionStringBuilder.OneSDbConnectionStringBuilder,
                        typeof(OneSDbProviderFactory).FullName)
                },
                {
                    '1',
                    new Tuple<DbConnectionStringBuilder, string>(ConnectionStringBuilder.WebServiceConnectionStringBuilder,
                        typeof(WebServiceDbProviderFactory).FullName)
                },
                {
                    '2',
                    new Tuple<DbConnectionStringBuilder, string>(ConnectionStringBuilder.WebServiceConnectionStringBuilder,
                        "Web service")
                },
            };

        const string Separator = "=======================";

        static void Main()
        {
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
                foreach (var item in Builders)
                {
                    Console.WriteLine(item.Key == '2'
                        ? $"[{item.Key}]: for [Web service]"
                        : $"[{item.Key}]: for [{item.Value.Item1.GetType().Name}]");
                }
                    

                var symbol = Console.ReadKey();
                Console.WriteLine("");
                if (!Builders.TryGetValue(symbol.KeyChar, out var tuple)) break;
                
                var builder = tuple.Item1;
                var factoryName = tuple.Item2;
                Console.WriteLine(Separator);
                Console.WriteLine($"Selected ConnectionString: {builder}");

                var sw = new Stopwatch();
                sw.Start();
                try
                {
                    if (symbol.KeyChar == '2')
                    {
                        TestWebServiceConnection();
                        continue;
                    }
                    
                    Console.WriteLine("Creating DbProviderFactory...");
                    var factory = DbProviderFactories.GetFactory(factoryName);
                    Console.WriteLine($"Factory created = {sw.Elapsed}");

                    Console.WriteLine($"Creating connection...");
                    using (var dbConnection = factory.CreateConnection())
                    {
                        Console.WriteLine($"Connection created = {sw.Elapsed}");
                        if (dbConnection == null) throw new NullReferenceException("Connection is null");

                        dbConnection.ConnectionString = builder.ConnectionString;
                        Console.WriteLine("Opening connection...");
                        dbConnection.Open();
                        Console.WriteLine($"Connection opened = {sw.Elapsed}");

                        Console.WriteLine("Creating command...");
                        using (var dbCommand = factory.CreateCommand())
                        {
                            Console.WriteLine($"Command created = {sw.Elapsed}");
                            if (dbCommand == null) throw new NullReferenceException("Command is null");

                            dbCommand.Connection = dbConnection;
                            dbCommand.CommandText = BasicQuery;
                            var numberOfRows = dbCommand.ExecuteNonQuery();

                            Console.WriteLine($"Number of rows = {numberOfRows}");
                            Console.WriteLine($"Elapsed = {sw.Elapsed}");
                        }
                        Console.WriteLine("Command disposed");
                    }
                    Console.WriteLine("Connection disposed");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("CONNECTION SUCCEEDED!");
                    Console.ResetColor();
                    Console.WriteLine($"Total time = {sw.Elapsed}");
                }
                catch (Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(exception);
                    Console.ResetColor();
                }
            }
        }

        static void TestWebServiceConnection()
        {
            try
            {
                Console.WriteLine("Executing web service request...");
                var sw = new Stopwatch();
                sw.Start();
                
                var serviceRequest = new WebServiceRequest(ConnectionStringBuilder.WebServiceConnectionStringBuilder, BasicQuery);
                serviceRequest.QueryData();
                Console.WriteLine($"Number of rows = {serviceRequest.ResulTable.Rows.Count}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("CONNECTION SUCCEEDED!");
                Console.ResetColor();
                Console.WriteLine($"Total time = {sw.Elapsed}");
            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exception);
                Console.ResetColor();
            }
        }
    }
}
