using System;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using Pike.OneS;
using Pike.OneS.Data;
using Pike.OneS.WebService;

namespace Pike.OneS.IntegrationTests
{
    class Program
    {
        #region DbCommandText

        const string DbCommandText = @"ВЫБРАТЬ
    ХозрасчетныйДвиженияССубконто.Период,
    ПРЕДСТАВЛЕНИЕ(ХозрасчетныйДвиженияССубконто.Регистратор),
	ХозрасчетныйДвиженияССубконто.НомерСтроки,
	ХозрасчетныйДвиженияССубконто.Активность,

    ПРЕДСТАВЛЕНИЕ(ХозрасчетныйДвиженияССубконто.СчетДт.Код) КАК СчетДтКод,
    ХозрасчетныйДвиженияССубконто.СчетДт.Наименование КАК СчетДтИмя,

    ПРЕДСТАВЛЕНИЕ(ХозрасчетныйДвиженияССубконто.СубконтоДт1.Код) КАК СубконтоДт1Код,
    ХозрасчетныйДвиженияССубконто.СубконтоДт1.Наименование КАК СубконтоДт1Имя,

    ПРЕДСТАВЛЕНИЕ(ХозрасчетныйДвиженияССубконто.СубконтоДт2.Код) КАК СубконтоДт2Код,
    ХозрасчетныйДвиженияССубконто.СубконтоДт2.Наименование КАК СубконтоДт2Имя,

    ПРЕДСТАВЛЕНИЕ(ХозрасчетныйДвиженияССубконто.СубконтоДт3.Код) КАК СубконтоДт3Код,
    ХозрасчетныйДвиженияССубконто.СубконтоДт3.Наименование КАК СубконтоДт3Имя,

    ПРЕДСТАВЛЕНИЕ(ХозрасчетныйДвиженияССубконто.ВидСубконтоДт1) КАК ВидСубконтоДт1,
    ПРЕДСТАВЛЕНИЕ(ХозрасчетныйДвиженияССубконто.ВидСубконтоДт2)КАК ВидСубконтоДт2,
    ПРЕДСТАВЛЕНИЕ(ХозрасчетныйДвиженияССубконто.ВидСубконтоДт3) КАК ВидСубконтоДт3,
    ХозрасчетныйДвиженияССубконто.СчетКт.Код КАК СчетКтКод,
    ХозрасчетныйДвиженияССубконто.СчетКт.Наименование КАК СчетКтИмя,
    ПРЕДСТАВЛЕНИЕ(ХозрасчетныйДвиженияССубконто.СубконтоКт1.Код) КАК СубконтоКт1Код,
    ХозрасчетныйДвиженияССубконто.СубконтоКт1.Наименование КАК СубконтоКт1Имя,
    ПРЕДСТАВЛЕНИЕ(ХозрасчетныйДвиженияССубконто.СубконтоКт2.Код) КАК СубконтоКт2Код,
    ХозрасчетныйДвиженияССубконто.СубконтоКт2.Наименование КАК СубконтоКт2Имя,
    ПРЕДСТАВЛЕНИЕ(ХозрасчетныйДвиженияССубконто.СубконтоКт3.Код) КАК СубконтоКт3Код,
    ХозрасчетныйДвиженияССубконто.СубконтоКт3.Наименование КАК СубконтоКт3Имя,
    ПРЕДСТАВЛЕНИЕ(ХозрасчетныйДвиженияССубконто.ВидСубконтоКт1) КАК ВидСубконтоКт1,
    ПРЕДСТАВЛЕНИЕ(ХозрасчетныйДвиженияССубконто.ВидСубконтоКт2) КАК ВидСубконтоКт2,
    ПРЕДСТАВЛЕНИЕ(ХозрасчетныйДвиженияССубконто.ВидСубконтоКт3) КАК ВидСубконтоКт3,
    ХозрасчетныйДвиженияССубконто.Организация.Наименование КАК ОрганизацияИмя,
    ХозрасчетныйДвиженияССубконто.ВалютаДт.Наименование КАК ВалютаДтИмя,
    ХозрасчетныйДвиженияССубконто.ВалютаКт.Наименование КАК ВалютаКтИмя,
    ХозрасчетныйДвиженияССубконто.Сумма,
    ХозрасчетныйДвиженияССубконто.КоличествоДт,
    ХозрасчетныйДвиженияССубконто.КоличествоКт,
    ХозрасчетныйДвиженияССубконто.ВалютнаяСуммаДт,
    ХозрасчетныйДвиженияССубконто.ВалютнаяСуммаКт,
    ХозрасчетныйДвиженияССубконто.СуммаНУДт,
    ХозрасчетныйДвиженияССубконто.СуммаНУКт,
    ХозрасчетныйДвиженияССубконто.Содержание,
    ХозрасчетныйДвиженияССубконто.ПодразделениеДт.Код КАК ПодразделениеДтКод,
    ХозрасчетныйДвиженияССубконто.ПодразделениеДт.Наименование КАК ПодразделениеДтИмя,
    ХозрасчетныйДвиженияССубконто.ПодразделениеКт.Код КАК ПодразделениеКтКод,
    ХозрасчетныйДвиженияССубконто.ПодразделениеКт.Наименование КАК ПодразделениеКтИмя,
    ПРЕДСТАВЛЕНИЕ(ТИПЗНАЧЕНИЯ(ХозрасчетныйДвиженияССубконто.Регистратор)) КАК ТипРегистратора
ИЗ

    РегистрБухгалтерии.Хозрасчетный.ДвиженияССубконто(
            ДАТАВРЕМЯ(2009, 1, 1, 0, 0, 0),
            КОНЕЦПЕРИОДА(ДАТАВРЕМЯ(2099, 3, 1, 0, 0, 0), ДЕНЬ),
            Активность = ИСТИНА
                И(СчетДт.Забалансовый = ЛОЖЬ
                    И СчетКт.Забалансовый = ЛОЖЬ),
            ,
            ) КАК ХозрасчетныйДвиженияССубконто";

        #endregion

        #region DbCommandRest

        const string DbCommandRest = @"ВЫБРАТЬ
	ХозрасчетныйОстаткиИОбороты.Счет.Код КАК СчетКод,
	ХозрасчетныйОстаткиИОбороты.Счет.Наименование КАК СчетИмя,
	ПРЕДСТАВЛЕНИЕ(ХозрасчетныйОстаткиИОбороты.Счет.Вид) КАК СчетВид,
	ХозрасчетныйОстаткиИОбороты.Счет.Забалансовый КАК СчетЗабалансовый,
	ПРЕДСТАВЛЕНИЕ(ХозрасчетныйОстаткиИОбороты.Субконто1.Код) КАК Субконто1Код,
	ХозрасчетныйОстаткиИОбороты.Субконто1.Наименование КАК Субконто1Имя,
	ПРЕДСТАВЛЕНИЕ(ТИПЗНАЧЕНИЯ(ХозрасчетныйОстаткиИОбороты.Субконто1)) КАК Субконто1Вид,
	ПРЕДСТАВЛЕНИЕ(ХозрасчетныйОстаткиИОбороты.Субконто2.Код) КАК Субконто2Код,
	ХозрасчетныйОстаткиИОбороты.Субконто2.Наименование КАК Субконто2Имя,
	ПРЕДСТАВЛЕНИЕ(ТИПЗНАЧЕНИЯ(ХозрасчетныйОстаткиИОбороты.Субконто2)) КАК Субконто2Вид,
	ПРЕДСТАВЛЕНИЕ(ХозрасчетныйОстаткиИОбороты.Субконто3.Код) КАК Субконто3Код,
	ХозрасчетныйОстаткиИОбороты.Субконто3.Наименование КАК Субконто3Имя,
	ПРЕДСТАВЛЕНИЕ(ТИПЗНАЧЕНИЯ(ХозрасчетныйОстаткиИОбороты.Субконто3)) КАК Субконто3Вид,
	ПРЕДСТАВЛЕНИЕ(ХозрасчетныйОстаткиИОбороты.Организация.Код) КАК ОрганизацияКод,
	ХозрасчетныйОстаткиИОбороты.Организация.Наименование КАК ОрганизацияИмя,
	ПРЕДСТАВЛЕНИЕ(ХозрасчетныйОстаткиИОбороты.Подразделение.Код) КАК ПодразделениеКод,
	ХозрасчетныйОстаткиИОбороты.Подразделение.Наименование КАК ПодразделениеИмя,
	ВЫБОР
		КОГДА ТИПЗНАЧЕНИЯ(ХозрасчетныйОстаткиИОбороты.Субконто1) = ТИП(Справочник.Контрагенты)
			ТОГДА ПРЕДСТАВЛЕНИЕ(ХозрасчетныйОстаткиИОбороты.Субконто1.Код)
		КОГДА ТИПЗНАЧЕНИЯ(ХозрасчетныйОстаткиИОбороты.Субконто2) = ТИП(Справочник.Контрагенты)
			ТОГДА ПРЕДСТАВЛЕНИЕ(ХозрасчетныйОстаткиИОбороты.Субконто2.Код)
		КОГДА ТИПЗНАЧЕНИЯ(ХозрасчетныйОстаткиИОбороты.Субконто3) = ТИП(Справочник.Контрагенты)
			ТОГДА ПРЕДСТАВЛЕНИЕ(ХозрасчетныйОстаткиИОбороты.Субконто3.Код)
		ИНАЧЕ NULL
	КОНЕЦ КАК КонтрагентыКод,
	ВЫБОР
		КОГДА ТИПЗНАЧЕНИЯ(ХозрасчетныйОстаткиИОбороты.Субконто1) = ТИП(Справочник.Контрагенты)
			ТОГДА ХозрасчетныйОстаткиИОбороты.Субконто1.Наименование
		КОГДА ТИПЗНАЧЕНИЯ(ХозрасчетныйОстаткиИОбороты.Субконто2) = ТИП(Справочник.Контрагенты)
			ТОГДА ХозрасчетныйОстаткиИОбороты.Субконто2.Наименование
		КОГДА ТИПЗНАЧЕНИЯ(ХозрасчетныйОстаткиИОбороты.Субконто3) = ТИП(Справочник.Контрагенты)
			ТОГДА ХозрасчетныйОстаткиИОбороты.Субконто3.Наименование
		ИНАЧЕ NULL
	КОНЕЦ КАК КонтрагентыИмя,
	ВЫБОР
		КОГДА ТИПЗНАЧЕНИЯ(ХозрасчетныйОстаткиИОбороты.Субконто1) = ТИП(Справочник.ДоговорыКонтрагентов)
			ТОГДА ПРЕДСТАВЛЕНИЕ(ХозрасчетныйОстаткиИОбороты.Субконто1.Код)
		КОГДА ТИПЗНАЧЕНИЯ(ХозрасчетныйОстаткиИОбороты.Субконто2) = ТИП(Справочник.ДоговорыКонтрагентов)
			ТОГДА ПРЕДСТАВЛЕНИЕ(ХозрасчетныйОстаткиИОбороты.Субконто2.Код)
		КОГДА ТИПЗНАЧЕНИЯ(ХозрасчетныйОстаткиИОбороты.Субконто3) = ТИП(Справочник.ДоговорыКонтрагентов)
			ТОГДА ПРЕДСТАВЛЕНИЕ(ХозрасчетныйОстаткиИОбороты.Субконто3.Код)
		ИНАЧЕ NULL
	КОНЕЦ КАК ДоговорыКод,
	ВЫБОР
		КОГДА ТИПЗНАЧЕНИЯ(ХозрасчетныйОстаткиИОбороты.Субконто1) = ТИП(Справочник.ДоговорыКонтрагентов)
			ТОГДА ХозрасчетныйОстаткиИОбороты.Субконто1.Наименование
		КОГДА ТИПЗНАЧЕНИЯ(ХозрасчетныйОстаткиИОбороты.Субконто2) = ТИП(Справочник.ДоговорыКонтрагентов)
			ТОГДА ХозрасчетныйОстаткиИОбороты.Субконто2.Наименование
		КОГДА ТИПЗНАЧЕНИЯ(ХозрасчетныйОстаткиИОбороты.Субконто3) = ТИП(Справочник.ДоговорыКонтрагентов)
			ТОГДА ХозрасчетныйОстаткиИОбороты.Субконто3.Наименование
		ИНАЧЕ NULL
	КОНЕЦ КАК ДоговорыИмя,
	ХозрасчетныйОстаткиИОбороты.СуммаНачальныйОстаток,
	ХозрасчетныйОстаткиИОбороты.СуммаНачальныйОстатокДт,
	ХозрасчетныйОстаткиИОбороты.СуммаНачальныйОстатокКт,
	ХозрасчетныйОстаткиИОбороты.СуммаНачальныйРазвернутыйОстатокДт,
	ХозрасчетныйОстаткиИОбороты.СуммаНачальныйРазвернутыйОстатокКт,
	ХозрасчетныйОстаткиИОбороты.СуммаКонечныйОстаток,
	ХозрасчетныйОстаткиИОбороты.СуммаКонечныйОстатокДт,
	ХозрасчетныйОстаткиИОбороты.СуммаКонечныйОстатокКт,
	ХозрасчетныйОстаткиИОбороты.СуммаКонечныйРазвернутыйОстатокДт,
	ХозрасчетныйОстаткиИОбороты.СуммаКонечныйРазвернутыйОстатокКт,
	ХозрасчетныйОстаткиИОбороты.СуммаОборотДт,
	ХозрасчетныйОстаткиИОбороты.СуммаОборотКт,
	ХозрасчетныйОстаткиИОбороты.КоличествоОборотДт,
	ХозрасчетныйОстаткиИОбороты.КоличествоОборотКт,
	ХозрасчетныйОстаткиИОбороты.КоличествоНачальныйРазвернутыйОстатокДт,
	ХозрасчетныйОстаткиИОбороты.КоличествоНачальныйРазвернутыйОстатокКт,
	ХозрасчетныйОстаткиИОбороты.КоличествоКонечныйРазвернутыйОстатокДт,
	ХозрасчетныйОстаткиИОбороты.КоличествоКонечныйРазвернутыйОстатокКт
ИЗ
	РегистрБухгалтерии.Хозрасчетный.ОстаткиИОбороты(ДАТАВРЕМЯ(2013, 1, 1, 0, 0, 0), КОНЕЦПЕРИОДА(ДАТАВРЕМЯ(2013, 12, 31, 0, 0, 0), ДЕНЬ), , , Счет.Забалансовый = ЛОЖЬ, , ) КАК ХозрасчетныйОстаткиИОбороты";

        #endregion

        #region DbCommandTextNom

        const string DbCommandTextNom = @"ВЫБРАТЬ
	Номенклатура.Код,
	Номенклатура.Наименование,
	Номенклатура.Артикул,
	Номенклатура.Комментарий
ИЗ
	Справочник.Номенклатура КАК Номенклатура";

        #endregion

        #region DbCommandTest

        const string DbCommandTest = @"ВЫБРАТЬ 1, ДАТАВРЕМЯ(2018, 03, 15, 0, 0, 0), ""Привет"", ИСТИНА";

        #endregion

        #region DbCommandTypes

        const string DbCommandTypes = @"ВЫБРАТЬ ПЕРВЫЕ 1000
	ХозрасчетныйДвиженияССубконто.Период,
	ПРЕДСТАВЛЕНИЕ(ХозрасчетныйДвиженияССубконто.Регистратор),
	ХозрасчетныйДвиженияССубконто.Активность,
	ХозрасчетныйДвиженияССубконто.Сумма
ИЗ
	РегистрБухгалтерии.Хозрасчетный.ДвиженияССубконто(
            ДАТАВРЕМЯ(2000, 1, 1, 0, 0, 0),
            КОНЕЦПЕРИОДА(ДАТАВРЕМЯ(2099, 3, 1, 0, 0, 0), ДЕНЬ),
			Активность = ИСТИНА
				И (СчетДт.Забалансовый = ЛОЖЬ
					И СчетКт.Забалансовый = ЛОЖЬ),
			,
			) КАК ХозрасчетныйДвиженияССубконто";

        #endregion

        #region DbStringBuilders

        static readonly OneSDbConnectionStringBuilder DbConnectionStringBuilder = new OneSDbConnectionStringBuilder
        {
            ProgId = "V83.ComConnector",
            Database = "KKU",
            Server = "WIN-U7DLHOF76FQ",
            User = "Ухов",
            Password = "кенгуру"
        };

        static readonly OneSDbConnectionStringBuilder DbConnectionWindowsStringBuilder = new OneSDbConnectionStringBuilder
        {
            ProgId = "V83.ComConnector",
            Database = "Accounting2Server",
            Server = "WIN-U7DLHOF76FQ",
            User = "Абдулов (директор)"
        };

        static readonly OneSDbConnectionStringBuilder DbWorkConnectionStringBuilder = new OneSDbConnectionStringBuilder
        {
            ProgId = "V83.COMConnector",
            Database = "overbest_em_bu_NFO",
            Server = "1C",
            User = "Integration",
            Password = "byntuhfnjh"
        };

        static readonly WebServiceConnectionStringBuilder WebServiceConnectionStringBuilder = new WebServiceConnectionStringBuilder
        {
            Address = "http://192.168.189.130",
            UriNamespace = "http://192.168.80.130",
            Database = "KKU",
            ServiceFileName = "wsIntegration.1cws",
            UserName = "Integration",
            Password = "Integration123"
        };

        #endregion

        #region FactoryNames

        static readonly string DbProviderFactoryName = typeof(OneSDbProviderFactory).FullName;
        static readonly string WebServiceProviderFactoryName = typeof(WebServiceDbProviderFactory).FullName;

        static readonly string[] FactoryNames = { DbProviderFactoryName, WebServiceProviderFactoryName };

        #endregion

        static void Main()
        {
            //Тестируем рабочий коннектор
            TestFactoryDbProvider(DbProviderFactoryName, DbWorkConnectionStringBuilder, DbCommandTest);
            
            //Типизированный DbProvider
            /*Console.WriteLine("Типизированный DbProvider (Логин 1С, тестовый запрос)");
            TestNativeDb(DbConnectionStringBuilder, DbCommandTest);
            Console.WriteLine("======================================================");

            Console.WriteLine("Типизированный DbProvider (Логин Windows, тестовый запрос)");
            TestNativeDb(DbConnectionWindowsStringBuilder, DbCommandTest);
            Console.WriteLine("======================================================");

            Console.WriteLine("Типизированный DbProvider (Логин 1С, РегистрБухгалтерии)");
            TestNativeDb(DbConnectionStringBuilder, DbCommandTypes);
            Console.WriteLine("======================================================");

            Console.WriteLine("Типизированный DbProvider (Логин Windows, РегистрБухгалтерии)");
            TestNativeDb(DbConnectionWindowsStringBuilder, DbCommandTypes);
            Console.WriteLine("======================================================");

            //Типизированный WebService
            Console.WriteLine("Типизированный WebService (Логин 1С, тестовый запрос)");
            TestWebService(WebServiceConnectionStringBuilder, DbCommandTest);
            Console.WriteLine("======================================================");

            Console.WriteLine("Типизированный WebService (Логин 1С, РегистрБухгалтерии)");
            TestNativeDb(DbConnectionStringBuilder, DbCommandTypes);
            Console.WriteLine("======================================================");

            //Незарегистрированынй OneSDbProviderFactory
            Console.WriteLine("Незарегистрированынй OneSDbProviderFactory (Логин 1С, тестовый запрос)");
            TestFactoryDbProvider(OneSDbProviderFactory.Instance, DbConnectionStringBuilder, DbCommandTest);
            Console.WriteLine("======================================================");

            Console.WriteLine("Незарегистрированынй OneSDbProviderFactory (Логин 1С, РегистрБухгалтерии)");
            TestFactoryDbProvider(OneSDbProviderFactory.Instance, DbConnectionStringBuilder, DbCommandTypes);
            Console.WriteLine("======================================================");

            //Незарегистрированынй WebServiceDbProviderFactory
            Console.WriteLine("Незарегистрированынй WebServiceDbProviderFactory (Логин 1С, тестовый запрос)");
            TestFactoryDbProvider(WebServiceDbProviderFactory.Instance, WebServiceConnectionStringBuilder, DbCommandTest);
            Console.WriteLine("======================================================");

            Console.WriteLine("Незарегистрированынй WebServiceDbProviderFactory (Логин 1С, РегистрБухгалтерии)");
            TestFactoryDbProvider(WebServiceDbProviderFactory.Instance, WebServiceConnectionStringBuilder, DbCommandTypes);
            Console.WriteLine("======================================================");

            //Зарегистрированный OneSDbProviderFactory
            Console.WriteLine("Зарегистрированный OneSDbProviderFactory (Логин 1С, тестовый запрос)");
            TestFactoryDbProvider(DbProviderFactoryName, DbConnectionStringBuilder, DbCommandTest);
            Console.WriteLine("======================================================");

            //Зарегистрированный WebServiceDbProviderFactory
            Console.WriteLine("Зарегистрированный WebServiceDbProviderFactory (Логин 1С, тестовый запрос)");
            TestFactoryDbProvider(WebServiceProviderFactoryName, WebServiceConnectionStringBuilder, DbCommandTest);
            Console.WriteLine("======================================================");*/

            //CoalDebug.Start();

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        static void TestNative(OneSConnectionStringBuilder builder, string query)
        {
            var sw = new Stopwatch();
            sw.Start();

            using (var dbConnection = new OneSConnector())
            {
                Console.WriteLine($"Create connection = {sw.Elapsed}");

                dbConnection.Connect(builder);
                using (var dbCommand = new OneSQuery(dbConnection))
                {
                    Console.WriteLine($"Create command = {sw.Elapsed}");

                    dbCommand.Text = DbCommandRest;
                    using (var queryResult = dbCommand.Execute())
                    {
                        Console.WriteLine($"Execute = {sw.Elapsed}");
                        var tbl = queryResult.ToDataTable();
                        Console.WriteLine($"Read = {sw.Elapsed}; Rows = {tbl.Rows.Count}");
                    }
                }
            }

            sw.Stop();
            Console.WriteLine($"Takes = {sw.Elapsed}");
        }

        static void TestSerialization(OneSConnectionStringBuilder connectionStringBuilder, string query)
        {
            var sw = new Stopwatch();
            sw.Start();

            using (var dbConnection = new OneSConnector())
            {
                Console.WriteLine($"Create connection = {sw.Elapsed}");

                dbConnection.Connect(connectionStringBuilder);
                using (var dbCommand = new OneSQuery(dbConnection))
                {
                    Console.WriteLine($"Create command = {sw.Elapsed}");
                    dbCommand.Text = query;
                    using (var queryResult = dbCommand.Execute())
                    {
                        Console.WriteLine($"Execute = {sw.Elapsed}");
                        var tbl = queryResult.DeserializeFromValueTable();
                        Console.WriteLine($"Read = {sw.Elapsed}; Rows = {tbl.Rows.Count}");
                    }
                }
            }

            sw.Stop();
            Console.WriteLine($"Takes = {sw.Elapsed}");
        }

        static void TestDbStorageStructureInfo(OneSConnectionStringBuilder connectionStringBuilder)
        {
            const string file = "Tables.xml";
            if (File.Exists(file))
                File.Delete(file);

            using (var connector = new OneSConnector())
            {
                connector.Connect(connectionStringBuilder);
                using (var vt = connector.GetDbStorageStructureInfo(null, true))
                {
                    var xmlString = connector.SerializeToXml(vt);
                    var xdocument = XDocument.Parse(xmlString);
                    xdocument.Save(file);
                }
            }
        }

        static void TestNativeDb(OneSDbConnectionStringBuilder builder, string query)
        {
            var sw = new Stopwatch();
            sw.Start();

            using (var dbConnection = new OneSDbConnection())
            {
                var cnt = 0;
                Console.WriteLine($"Create connection = {sw.Elapsed}");

                dbConnection.ConnectionString = builder.ConnectionString;
                dbConnection.Open();
                using (var dbCommand = new OneSDbCommand())
                {
                    Console.WriteLine($"Create command = {sw.Elapsed}");

                    dbCommand.Connection = dbConnection;
                    dbCommand.CommandText = query;
                    using (var dbReader = dbCommand.ExecuteReader())
                    {
                        Console.WriteLine($"Execute reader = {sw.Elapsed}");
                        while (dbReader.Read())
                        {
                            var values = new object[dbReader.FieldCount];
                            dbReader.GetValues(values);

                            cnt++;
                        }
                        Console.WriteLine($"Number of rows = {cnt}");
                        Console.WriteLine($"Read = {sw.Elapsed}");
                    }
                }
            }

            sw.Stop();
            Console.WriteLine($"Takes = {sw.Elapsed}");
        }

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

        static void TestFactoryDbProvider(DbProviderFactory factory, DbConnectionStringBuilder builder, string query)
        {
            var cnt = 0;
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                using (var dbConnection = factory.CreateConnection())
                {
                    Console.WriteLine($"Create connection = {sw.Elapsed}");

                    if (dbConnection == null) throw new NullReferenceException();

                    dbConnection.ConnectionString = builder.ConnectionString;
                    dbConnection.Open();
                    using (var dbCommand = factory.CreateCommand())
                    {
                        Console.WriteLine($"Create command = {sw.Elapsed}");
                        if (dbCommand == null) throw new NullReferenceException();

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = query;
                        using (var dbReader = dbCommand.ExecuteReader())
                        {
                            Console.WriteLine($"Execute reader = {sw.Elapsed}");
                            while (dbReader.Read())
                            {
                                var values = new object[dbReader.FieldCount];
                                dbReader.GetValues(values);

                                cnt++;
                            }
                            Console.WriteLine($"Number of rows = {cnt}");
                            Console.WriteLine($"Read = {sw.Elapsed}");
                        }
                        Console.WriteLine("Reader closed");
                    }
                    Console.WriteLine("Command closed");
                }
                Console.WriteLine("Connection closed");
            }
            catch (Exception exception)
            {
                Console.WriteLine("EXCEPTION:");
                Console.WriteLine(exception.ToString());
            }

            sw.Stop();
            Console.WriteLine($"Takes = {sw.Elapsed}");
        }

        static void TestFactoryDbProvider(string factoryName, DbConnectionStringBuilder builder, string query)
        {
            var cnt = 0;
            var sw = new Stopwatch();
            sw.Start();

            var factory = DbProviderFactories.GetFactory(factoryName);
            Console.WriteLine($"Get factory = {sw.Elapsed}");

            using (var dbConnection = factory.CreateConnection())
            {
                Console.WriteLine($"Create connection = {sw.Elapsed}");

                if (dbConnection == null) throw new NullReferenceException();

                dbConnection.ConnectionString = builder.ConnectionString;
                dbConnection.Open();
                using (var dbCommand = factory.CreateCommand())
                {
                    Console.WriteLine($"Create command = {sw.Elapsed}");
                    if (dbCommand == null) throw new NullReferenceException();

                    dbCommand.Connection = dbConnection;
                    dbCommand.CommandText = query;
                    using (var dbReader = dbCommand.ExecuteReader())
                    {
                        Console.WriteLine($"Execute reader = {sw.Elapsed}");
                        while (dbReader.Read())
                        {
                            var values = new object[dbReader.FieldCount];
                            dbReader.GetValues(values);

                            cnt++;
                        }
                        Console.WriteLine($"Number of rows = {cnt}");
                        Console.WriteLine($"Read = {sw.Elapsed}");
                    }
                }
            }

            sw.Stop();
            Console.WriteLine($"Takes = {sw.Elapsed}");
        }
    }
}