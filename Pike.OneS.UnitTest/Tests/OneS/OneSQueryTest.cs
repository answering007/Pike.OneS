using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System;
using System.Linq;

namespace Pike.OneS.UnitTest.Tests.OneS
{
    [TestClass]
    public class OneSQueryTest
    {
        [TestMethod]
        public void TestOneSQueryResultDeserializeFromValueTable()
        {
            using (var dbConnection = new OneSConnector())
            {
                dbConnection.Connect(ConnectionStringBuilder.OneSConnectionStringBuilder);
                using (var dbCommand = new OneSQuery(dbConnection))
                {
                    dbCommand.Text = TestHelper.BasicQuery;

                    using (var queryResult = dbCommand.Execute())
                    {
                        var tbl = queryResult.DeserializeFromValueTable();
                        TestHelper.BasicCompare(tbl);
                    }
                }
            }
        }

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
        public void Test()
        {
            using (var dbConnection = new OneSConnector())
            {
                dbConnection.Connect(ConnectionStringBuilder.OneSConnectionStringBuilder);
                using (var dbCommand = new OneSQuery(dbConnection))
                {
                    var query = @"ВЫБРАТЬ ПЕРВЫЕ 10
	Банки.Ссылка,
	Банки.Код,
	Банки.Наименование
ИЗ
	Справочник.Банки КАК Банки";
                    dbCommand.Text = query;

                    using (var queryResult = dbCommand.Execute())
                    {
                        var tbl = queryResult.DeserializeFromValueTable();
                        PrintDataTable(tbl, TestContext);
                        Assert.IsNotNull(tbl);
                    }
                }
            }
        }
    }
}
