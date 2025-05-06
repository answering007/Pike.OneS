using Pike.OneS.WebService;
using System;
using System.Data.Common;
using System.Diagnostics;

namespace Pike.ConnectionTest.ConnectionInfo
{
    public class WebServiceConnectionInfo: IConnectionInfo
    {
        public string Name { get; }
        public char Id { get; }
        public DbConnectionStringBuilder Builder { get; } = ConnectionStringBuilder.WebServiceConnectionStringBuilder;

        public WebServiceConnectionInfo(string name, char id)
        {
            Name = name;
            Id = id;
        }

        public void TestConnection()
        {
            try
            {
                Console.WriteLine("Executing web service request...");
                var sw = new Stopwatch();
                sw.Start();

                var serviceRequest = new WebServiceRequest((WebServiceConnectionStringBuilder)Builder, FactoryConnectionInfo.BasicQuery);
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