# .Net assembly to get data from 1C
-----

**Table of Contents**

- [.Net assembly to get data from 1C](#.Net-assembly-to-get-data-from-1C)
  - [Overview](#overview)
  - [Installation](#installation)
  - [Usage](#usage)
      - [Late binding wrapper](#Late-binding-wrapper)
      - [ADO.Net provider based on wrapper](#ADO.Net-provider-based-on-wrapper)
  - [License](#license)

## Overview

This provider use ComСonnector and WebServise to get data from 1C database:
1. Late binding wrapper for 1C COM Connector type library
2. ADO.Net provider based on wrapper
3. ADO.Net provider based on 1C webservice

## Installation

1. Download latest release
2. Copy **messageserver-X.jar** file to **SpigotServer\plugins** folder
3. Start spigot server or send `reload` command from server console

## Usage

### Late binding wrapper:
Basic example:
```csharp
using (var dbConnection = new OneSConnector())
{
    // Create connection string builder
    var builder = new OneSDbConnectionStringBuilder
    {
        ProgId = "V83.ComConnector",
        Database = "Name of the database",
        Server = "Name of the server",
        User = "User name",
        Password = "Strong password"
    };
    
    // Connect to the database
    dbConnection.Connect(builder);
    
    // Connect command
    using (var dbCommand = new OneSQuery(dbConnection))
    {
        // Set command text
        dbCommand.Text = query;

        // Execute command and parse the result
        using (var queryResult = dbCommand.Execute())
        {
            var dataTable = queryResult.DeserializeFromValueTable();
            Console.WriteLine($"Rows = {dataTable.Rows.Count}");
        }
    }
}
```

Get database storage structure:
```csharp
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
```

### ADO.Net provider based on wrapper:
```csharp
using (var dbConnection = new OneSDbConnection())
{
    var builder = new OneSDbConnectionStringBuilder
    {
        ProgId = "V83.ComConnector",
        Database = "Name of the database",
        Server = "Name of the server",
        User = "User name",
        Password = "Strong password"
    };

    var cnt = 0;

    dbConnection.ConnectionString = builder.ConnectionString;
    dbConnection.Open();

    using (var dbCommand = new OneSDbCommand())
    {
        dbCommand.Connection = dbConnection;
        dbCommand.CommandText = query;
        
        using (var dbReader = dbCommand.ExecuteReader())
        {
            while (dbReader.Read())
            {
                var values = new object[dbReader.FieldCount];
                dbReader.GetValues(values);

                cnt++;
            }
            Console.WriteLine($"Number of rows = {cnt}");
        }
    }
}
```
## License

`messageserver` is distributed under the terms of the [MIT](https://spdx.org/licenses/MIT.html) license.