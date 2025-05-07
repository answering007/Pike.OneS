using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pike.OneS.WebService;
using System.Data;
using System.Linq;

namespace Pike.OneS.UnitTest.Tests
{
    [TestClass]
    public class WebServiceTest
    {
        public TestContext TestContext { get; set; }

        static void PrintDataTable(DataTable table, TestContext context)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            if (context == null) throw new ArgumentNullException(nameof(context));

            var columns = table.Columns.Cast<DataColumn>();
            context.WriteLine(string.Join("\t\t", columns));

            var rows = table.Rows.Cast<DataRow>();
            foreach (var row in rows)
                context.WriteLine(string.Join("\t\t", row.ItemArray));
        }
        
        [TestMethod]
        public void TestWebServiceWithLoginAndPassword()
        {
            var serviceRequest = new WebServiceRequest(ConnectionStringBuilder.WebServiceConnectionStringBuilder, TestHelper.BasicQuery);
            serviceRequest.QueryData();
            TestHelper.BasicCompare(serviceRequest.ResulTable);
        }

        [TestMethod]
        public void Test01()
        {
            var query = @"ВЫБРАТЬ ПЕРВЫЕ 10
	ХозрасчетныйДвиженияССубконто.Период,
	ХозрасчетныйДвиженияССубконто.Регистратор,
	ХозрасчетныйДвиженияССубконто.СчетДт,
	ХозрасчетныйДвиженияССубконто.СчетКт,
	ХозрасчетныйДвиженияССубконто.Сумма
ИЗ
	РегистрБухгалтерии.Хозрасчетный.ДвиженияССубконто КАК ХозрасчетныйДвиженияССубконто";
            var serviceRequest = new WebServiceRequest(ConnectionStringBuilder.WebServiceConnectionStringBuilder, query);
            serviceRequest.QueryData();
            PrintDataTable(serviceRequest.ResulTable, TestContext);
            Assert.IsNotNull(serviceRequest.ResulTable);
        }

        [TestMethod]
        public void TestWebServiceWithLoginOnly()
        {
            var builder = ConnectionStringBuilder.WebServiceConnectionStringBuilder;
            builder.UserName = "LoginOnlyUser";
            builder.Password = null;
            var serviceRequest = new WebServiceRequest(builder, TestHelper.BasicQuery);
            serviceRequest.QueryData();
            TestHelper.BasicCompare(serviceRequest.ResulTable);
        }

        [TestMethod]
        public void TestOneSDbCommandResult()
        {
            using (var dbConnection = new WebServiceConnection())
            {
                dbConnection.ConnectionString = ConnectionStringBuilder.WebServiceConnectionStringBuilder.ConnectionString;
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.Connection = dbConnection;
                    dbCommand.CommandText = TestHelper.BasicQuery;
                    var result = new DataTable();
                    using (var dbReader = dbCommand.ExecuteReader())
                    {
                        result.Load(dbReader);
                        TestHelper.BasicCompare(result);
                    }
                }
            }
        }
    }
}
