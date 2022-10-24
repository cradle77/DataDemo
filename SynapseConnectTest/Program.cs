// See https://aka.ms/new-console-template for more information
using Azure.Identity;
using System.Data.SqlClient;

var connectionString = "{your connection string goes here}";

// connect via managed identity
var connection = new SqlConnection(connectionString);

var credential = new DefaultAzureCredential();

var cancellation = new CancellationTokenSource();
var token = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" }), cancellation.Token);
connection.AccessToken = token.Token;

connection.Open();

var cmd = connection.CreateCommand();

cmd.CommandTimeout = 60;

cmd.CommandText = "SELECT code_gender, MIN(days_employed), MAX(days_employed) FROM\r\n    OPENROWSET(\r\n        BULK 'abfss://datafiles@desdatademo.dfs.core.windows.net/raw/application_record_new.csv',\r\n        FORMAT='CSV',\r\n        parser_version = '2.0',\r\n        HEADER_ROW = TRUE\r\n    ) AS applications\r\ngroup by code_gender";

using (var reader = cmd.ExecuteReader())
{
    while (reader.Read())
    {
        Console.WriteLine($"{reader[0]}: {reader[1]}, {reader[2]}");
    }
}

