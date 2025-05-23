﻿# Провайдер .NET Framework для получения данных из 1С
-----

## Содержание
<!--TOC-->
  - [Обзор](#обзор)
  - [Установка](#установка)
  - [Использование](#использование)
    - [.NET провайдер на основе 1С ComConnector](#net-%D0%BF%D1%80%D0%BE%D0%B2%D0%B0%D0%B9%D0%B4%D0%B5%D1%80-%D0%BD%D0%B0-%D0%BE%D1%81%D0%BD%D0%BE%D0%B2%D0%B5-1%D1%81-comconnector)
      - [Формирование запроса и получение данных](#%D1%84%D0%BE%D1%80%D0%BC%D0%B8%D1%80%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5-%D0%B7%D0%B0%D0%BF%D1%80%D0%BE%D1%81%D0%B0-%D0%B8-%D0%BF%D0%BE%D0%BB%D1%83%D1%87%D0%B5%D0%BD%D0%B8%D0%B5-%D0%B4%D0%B0%D0%BD%D0%BD%D1%8B%D1%85)
      - [Получение структуры БД](#%D0%BF%D0%BE%D0%BB%D1%83%D1%87%D0%B5%D0%BD%D0%B8%D0%B5-%D1%81%D1%82%D1%80%D1%83%D0%BA%D1%82%D1%83%D1%80%D1%8B-%D0%B1%D0%B4)
    - [ADO.NET провайдер на основе 1С ComConnector](#adonet-%D0%BF%D1%80%D0%BE%D0%B2%D0%B0%D0%B9%D0%B4%D0%B5%D1%80-%D0%BD%D0%B0-%D0%BE%D1%81%D0%BD%D0%BE%D0%B2%D0%B5-1%D1%81-comconnector)
      - [Формирование запроса и получение данных](#%D1%84%D0%BE%D1%80%D0%BC%D0%B8%D1%80%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5-%D0%B7%D0%B0%D0%BF%D1%80%D0%BE%D1%81%D0%B0-%D0%B8-%D0%BF%D0%BE%D0%BB%D1%83%D1%87%D0%B5%D0%BD%D0%B8%D0%B5-%D0%B4%D0%B0%D0%BD%D0%BD%D1%8B%D1%85-1)
    - [Web service 1С](#web-service-1%D1%81)
      - [Использование Web service](#%D0%B8%D1%81%D0%BF%D0%BE%D0%BB%D1%8C%D0%B7%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5-web-service)
      - [Использование ADO.NET провайдера на основе Web service](#%D0%B8%D1%81%D0%BF%D0%BE%D0%BB%D1%8C%D0%B7%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5-adonet-%D0%BF%D1%80%D0%BE%D0%B2%D0%B0%D0%B9%D0%B4%D0%B5%D1%80%D0%B0-%D0%BD%D0%B0-%D0%BE%D1%81%D0%BD%D0%BE%D0%B2%D0%B5-web-service)
  - [Проверка подключения](#%D0%BF%D1%80%D0%BE%D0%B2%D0%B5%D1%80%D0%BA%D0%B0-%D0%BF%D0%BE%D0%B4%D0%BA%D0%BB%D1%8E%D1%87%D0%B5%D0%BD%D0%B8%D1%8F)
  - [Power Query и другие ETL инструменты](#power-query-%D0%B8-%D0%B4%D1%80%D1%83%D0%B3%D0%B8%D0%B5-etl-%D0%B8%D0%BD%D1%81%D1%82%D1%80%D1%83%D0%BC%D0%B5%D0%BD%D1%82%D1%8B)
  - [Дополнительные материалы](#%D0%B4%D0%BE%D0%BF%D0%BE%D0%BB%D0%BD%D0%B8%D1%82%D0%B5%D0%BB%D1%8C%D0%BD%D1%8B%D0%B5-%D0%BC%D0%B0%D1%82%D0%B5%D1%80%D0%B8%D0%B0%D0%BB%D1%8B)
<!--/TOC-->
## Обзор

**Реализовано несколько механизмов получения данных из 1С:**
1. .NET провайдер на основе 1С ComConnector с использованием технологии позднего связывания (Late binding)
2. ADO.NET провайдер на основе 1С ComConnector с использованием технологии позднего связывания (Late binding)
3. Web service 1С
4. ADO.NET провайдер на основе Web service.
Данный провайдер сделан для ускорения преобразования xml-значений в таблицу в случае, если встроенные инструменты ETL не позволяют сделать это эффективно.
Например, в Power Query ответ Web service быстрее обработать через ADO.NET в случае запроса большого набора данных.

Для примитивных типов:
- boolean
- DateTime
- decimal
- string

возвращаются соответствющие значения, для ссылочных типов возвращается **UUID (Universally Unique Identifier)**, для перечислений: **Имя**

При установке ADO.NET провайдеры регистрируются в:
* ```%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\Config\machine.config```
* ```%SYSTEMROOT%\Microsoft.NET\Framework64\v4.0.30319\Config\machine.config```

Обращение к провайдерам возможно через фабрики ```DbProviderFactories.GetFactory(factoryName)```

## Установка
1. Загрузите последний релиз
2. Запустите ```setup.exe``` от имени ```администратора```

**[Создание Web service через конфигуратор 1С](Pike.OneS/Info/InstallWebService.md)**

## Использование

### .NET провайдер на основе 1С ComConnector
#### Формирование запроса и получение данных
```csharp
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
```
#### Получение структуры БД
```csharp
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
```

### ADO.NET провайдер на основе 1С ComConnector
#### Формирование запроса и получение данных
```csharp
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
```

### Web service 1С
#### Использование Web service
Формирование запроса на получение данных:
```csharp
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
```

#### Использование ADO.NET провайдера на основе Web service

Использование провайдера аналогично использованию ADO.NET провайдера на основе ComConnector:

- ```WebServiceConnectionStringBuilder```- для проектирования строки подключения
- ```WebServiceConnection```- для создания подключения
- ```WebServiceCommand```- для запроса данных

## Проверка подключения
Для проверки подключения можно воспользоваться утилитой ```Pike.ConnectionTest.exe```
В настройках утилиты присутствуют 2 секции параметров:
- ```SettingsConnection```- настройки провайдера ADO.NET на основе 1С ComConnector
- ```SettingsWebService```- настройки Web service и провайдера ADO.NET на основе Web service

Настройки для тестирования необходимо установить до запуска приложения.
При запуске приложения необходимо выбрать символ, который соответствует тестируемому подключению.
В случае успешного подключения будет выведено сообщение <span style="color:green">```CONNECTION SUCCEEDED!```</span>.


## Power Query и другие ETL инструменты
[Power Query](https://learn.microsoft.com/en-us/powerquery-m/) и другие средства интеграции, поддерживающие работу с ADO.NET могут быть использованы для получения данных:
```fsharp
let
    query = AdoDotNet.Query(
        "Pike.OneS.WebService.WebServiceDbProviderFactory", 
        "Address=http://192.168.189.129;UriNamespace=http://10.10.15.150/WebIntegration;Database=AccountingServer;ServiceFileName=WebIntegration.1cws;UserName=Integration",
        "ВЫБРАТЬ 1 КАК Номер, ДАТАВРЕМЯ(2018, 03, 15, 0, 0, 0) КАК Дата, ""Привет"" КАК Строка, ИСТИНА КАК Флаг")
in
    query
```
В папке проекта [Excel](Pike.OneS/Info/Excel) отражены примеры на Power Query для получения данных через:
- ADO.NET ComConnector
- Web service
- ADO.NET Web service

## Дополнительные материалы
- Обработка [консоль запросов универсальная](Pike.OneS/Info/КонсольЗапросовУниверсальная.epf)
- [Создать оболочку COM+ для 1С (Платформа 1С x64)](https://saby.ru/help/integration/1C_set/64bit)
- [Настройка веб сервера Apache + 1С (Пошаговое руководство)](https://infostart.ru/1c/articles/646384/)