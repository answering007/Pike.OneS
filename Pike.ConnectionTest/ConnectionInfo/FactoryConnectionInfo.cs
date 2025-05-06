using System;
using System.Data.Common;
using System.Diagnostics;

namespace Pike.ConnectionTest.ConnectionInfo
{
    public class FactoryConnectionInfo: IConnectionInfo
    {
        public const string BasicQuery = @"ВЫБРАТЬ 1, ДАТАВРЕМЯ(2018, 03, 15, 1, 2, 3), ""Привет"", ИСТИНА";
        
        public string Name { get; }
        public char Id { get; }
        public DbConnectionStringBuilder Builder { get; }
        public string FactoryName { get; }

        public FactoryConnectionInfo(string name, char id, DbConnectionStringBuilder builder, string factoryName)
        {
            Name = name;
            Id = id;
            Builder = builder;
            FactoryName = factoryName;
        }

        public void TestConnection()
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                Console.WriteLine("Creating DbProviderFactory...");
                var factory = DbProviderFactories.GetFactory(FactoryName);
                Console.WriteLine($"Factory created = {sw.Elapsed}");

                Console.WriteLine($"Creating connection...");
                using (var dbConnection = factory.CreateConnection())
                {
                    Console.WriteLine($"Connection created = {sw.Elapsed}");
                    if (dbConnection == null) throw new NullReferenceException("Connection is null");

                    dbConnection.ConnectionString = Builder.ConnectionString;
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
}