using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.Common;

namespace Pike.OneS.UnitTest
{
    internal static class UnitTestHelper
    {
        public const string BasicQuery = @"ВЫБРАТЬ 1, ДАТАВРЕМЯ(2018, 03, 15, 1, 2, 3), ""Привет"", ИСТИНА";

        public static void BasicCompare(DataTable dataTable)
        {
            Assert.IsNotNull(dataTable);

            var decimalValue = (decimal)dataTable.Rows[0][0];
            var dateTimeValue = (DateTime)dataTable.Rows[0][1];
            var stringValue = (string)dataTable.Rows[0][2];
            var boolValue = (bool)dataTable.Rows[0][3];

            Assert.AreEqual(1m, decimalValue);
            Assert.AreEqual(new DateTime(2018, 03, 15, 1, 2, 3), dateTimeValue);
            Assert.AreEqual("Привет", stringValue);
            Assert.AreEqual(true, boolValue);
        }

        public static DataTable GetData(DbProviderFactory factory, DbConnectionStringBuilder builder, string query)
        {
            var dataTable = new DataTable();

            using (var dbConnection = factory.CreateConnection())
            {
                if (dbConnection == null) throw new NullReferenceException();

                dbConnection.ConnectionString = builder.ConnectionString;
                dbConnection.Open();
                using (var dbCommand = factory.CreateCommand())
                {
                    if (dbCommand == null) throw new NullReferenceException();

                    dbCommand.Connection = dbConnection;
                    dbCommand.CommandText = query;
                    using (var dbReader = dbCommand.ExecuteReader())
                        dataTable.Load(dbReader);
                }
            }
            return dataTable;
        }
    }
}