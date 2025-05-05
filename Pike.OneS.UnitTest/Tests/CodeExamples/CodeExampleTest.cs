using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Xml.Linq;
using Pike.OneS.Data;
using Pike.OneS.WebService;

namespace Pike.OneS.UnitTest.Tests.CodeExamples
{
    [TestClass]
    public class CodeExampleTest
    {
        [TestMethod]
        public void DotNetTest()
        {
            // Проектируем строку подключения к 1С
            var builder = new OneSConnectionStringBuilder
            {
                Srvr = "192.168.189.129",   // Адрес сервера
                Ref = "AccountingServer",   // Имя базы данных
                Usr = "Integration",        // Имя пользователя
                Pwd = "Integration123"      // Пароль
                //File = "...",             // Для файл-серверного варианта
            };

            // Создаем объект подключения
            using (var dbConnection = new OneSConnector())
            {
                // Подключаемся
                dbConnection.Connect(builder);

                // Создаем запрос
                using (var dbCommand = new OneSQuery(dbConnection))
                {
                    // Устанавливаем текст запроса
                    dbCommand.Text = @"ВЫБРАТЬ 1, ДАТАВРЕМЯ(2018, 03, 15, 1, 2, 3), ""Привет"", ИСТИНА";

                    // Выполняем запрос
                    using (var queryResult = dbCommand.Execute())
                    {
                        // Получаем результат в виде объекта DataTable
                        var dataTable = queryResult.DeserializeFromValueTable();

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
                }
            }
        }

        [TestMethod]
        public void DotNetDbStorageStructureInfo()
        {
            // Проектируем строку подключения к 1С
            var builder = new OneSConnectionStringBuilder
            {
                Srvr = "192.168.189.129",   // Адрес сервера
                Ref = "AccountingServer",   // Имя базы данных
                Usr = "Integration",        // Имя пользователя
                Pwd = "Integration123"      // Пароль
                //File = "...",             // Для файл-серверного варианта
            };

            // Создаем объект подключения
            using (var connector = new OneSConnector())
            {
                // Подключаемся
                connector.Connect(builder);
                // Получаем информацию о структуре БД
                using (var valueTable = connector.GetDbStorageStructureInfo(null, true))
                {
                    // Сериализуем в Xml
                    var xmlString = connector.SerializeToXml(valueTable);
                    // Формируем XDocument
                    var document = XDocument.Parse(xmlString);

                    Assert.IsNotNull(document);
                }
            }
        }

        [TestMethod]
        public void AdoDotNetTest()
        {
            // Проектируем строку подключения к 1С
            var builder = new OneSDbConnectionStringBuilder()
            {
                ProgId = "V83.ComConnector",// Идентификатор COM-объекта
                Srvr = "192.168.189.129",   // Адрес сервера
                Ref = "AccountingServer",   // Имя базы данных
                Usr = "Integration",        // Имя пользователя
                Pwd = "Integration123"      // Пароль
            };

            // Создаем объект подключения
            using (var dbConnection = new OneSDbConnection())
            {
                // Устанавливаем строку подключения
                dbConnection.ConnectionString = builder.ConnectionString;
                // Подключаемся
                dbConnection.Open();

                // Создаем запрос
                using (var dbCommand = new OneSDbCommand())
                {
                    dbCommand.Connection = dbConnection;
                    // Устанавливаем текст запроса
                    dbCommand.CommandText = @"ВЫБРАТЬ 1, ДАТАВРЕМЯ(2018, 03, 15, 1, 2, 3), ""Привет"", ИСТИНА";

                    // Выполняем запрос
                    using (var queryResult = dbCommand.ExecuteReader())
                    {
                        // Получаем результат в виде объекта DataTable
                        var dataTable = new DataTable();
                        dataTable.Load(queryResult);

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
                }
            }
        }

        [TestMethod]
        public void WebServiceTest()
        {
            var builder = new WebServiceConnectionStringBuilder
            {
                Address = "http://192.168.189.129",                     // Адрес сервера
                UriNamespace = "http://10.10.15.150/WebIntegration",    // URI пространства имен
                Database = "AccountingServer",                          // Имя БД
                ServiceFileName = "WebIntegration.1cws",                // Имя файла публикации
                UserName = "Integration",                               // Имя пользователя
                Password = "Integration123"                             // Пароль
            };

            const string query = @"ВЫБРАТЬ 1, ДАТАВРЕМЯ(2018, 03, 15, 1, 2, 3), ""Привет"", ИСТИНА";

            var serviceRequest = new WebServiceRequest(builder, query);
            serviceRequest.QueryData();
            var dataTable = serviceRequest.ResulTable;
            
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
    }
}
