using System;
using System.Diagnostics;
using Pike.OneS.WebService;

namespace Pike.OneS.IntegrationTests
{
    public class CoalDebug
    {
        #region DbCommandRest

        const string DbCommandRest = @"ВЫБРАТЬ
 Представление(СтавкиНДС.Ссылка),
 Представление(СтавкиНДС.Порядок)
ИЗ
 Перечисление.СтавкиНДС КАК СтавкиНДС";

        #endregion

        static readonly WebServiceConnectionStringBuilder WebServiceConnectionStringBuilder = new WebServiceConnectionStringBuilder
        {
            Address = "http://94.251.4.234",
            UriNamespace = "http://1C/ws1",
            Database = "integration",
            ServiceFileName = "ws1.1cws",
            UserName = "Integration",
            Password = "[ПарольК1С]"
        };

        static void TestWebService(WebServiceConnectionStringBuilder builder, string query)
        {
            var sw = new Stopwatch();
            sw.Start();

            var serviceRequest = new WebServiceRequest(builder, query);
            serviceRequest.QueryData();

            sw.Stop();
            Console.WriteLine("Columns: {0}; Rows: {1}", serviceRequest.ResulTable.Columns.Count, serviceRequest.ResulTable.Rows.Count);
            Console.WriteLine($"Takes = {sw.Elapsed}");
        }


        public static void Start()
        {
            TestWebService(WebServiceConnectionStringBuilder, DbCommandRest);
        }
    }
}