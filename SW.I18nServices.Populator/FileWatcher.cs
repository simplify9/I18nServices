using CsvHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using SW.I18nServices.Api.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SW.I18nServices.Populator
{
    public class FileWatcher : IHostedService, IDisposable
    {
        private Timer timer;
        private IConfiguration configuration;
        private readonly ILogger logger;
        public FileWatcher(ILogger<FileWatcher> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }
        public void Dispose()
        {
            timer.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Started File Watcher");
            timer = new Timer(SyncFiles, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        public async void SyncFiles(object state)
        {
            var filePaths = await GetNewFiles();

            if (filePaths.Count == 0) return;

            var files = await DownloadFiles(filePaths);
            

        }

        public async Task<List<string>> GetNewFiles()
        {
            List<string> newFiles = new List<string>();
            // Get files from DO
            // Skip 1 to avoid space base url
            // return list
            return await Task.FromResult(newFiles);
        }

        public async Task<List<Stream>> DownloadFiles(List<string> filePaths)
        {
            List<Stream> files = new List<Stream>();
            foreach(var _ in filePaths)
            {
                //Download files, add them to files array
            }
            return await Task.FromResult(files);
        }

        private async static Task ExecuteSql(DbConnection sqlConnection, string sql)
        {
            using var command = sqlConnection.CreateCommand();
            command.CommandText = sql;
            await command.ExecuteNonQueryAsync();
        }


        public async Task PopulateTable(Stream csvFile)
        {
            using StreamReader reader = new StreamReader(csvFile);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Configuration.Delimiter = ";";
            csv.Configuration.RegisterClassMap<Place.Map>();
            csv.Configuration.TypeConverterOptionsCache.GetOptions<string>().NullValues.Add("");


            using var connection = new MySqlConnection
            {
                ConnectionString = configuration.GetConnectionString("")
            };

            await connection.OpenAsync();

            var firstRecord = csv.GetRecords<Place>().First();
            var tableName = $"Places_{firstRecord.CountryCode}";
            var tempTableName = $"Places_{firstRecord.CountryCode}_Temp";

            await ExecuteSql(connection, $@"
                DROP TABLE IF EXISTS {tempTableName};
                CREATE TABLE {tempTableName}(
	                Language char(2) NOT NULL,
	                Region1 nvarchar(80) NULL,
	                Region2 nvarchar(80) NULL,
	                Region3 nvarchar(80) NULL,
	                Region4 nvarchar(80) NULL,
	                Locality nvarchar(80) NULL,
	                Postcode nvarchar(15) NULL,
	                Suburb nvarchar(100) NULL,
	                Longitude decimal(9, 6) NULL,
	                Latitude decimal(9, 6) NULL
                );");

            //var lst = csv.GetRecords<Place>().ToList();

            StringBuilder insertStatementBuilder = null;
            var index = 0;

            foreach (var item in csv.GetRecords<Place>())
            {
                if (index == 0)
                {
                    insertStatementBuilder = new StringBuilder(
                        $@"INSERT INTO {tempTableName}(Language,Region1,Region2,Region3,Region4,
                        Locality,Postcode,Suburb,Longitude,Latitude) VALUES "
                    );
                }

                insertStatementBuilder.Append($"{(item.Language == null ? "(null," : $"('{item.Language}',")}");
                insertStatementBuilder.Append($"{(item.Region1 == null ? "null," : $"'{item.Region1.Replace("'", "''")}',")}");
                insertStatementBuilder.Append($"{(item.Region2 == null ? "null," : $"'{item.Region2.Replace("'", "''")}',")}");
                insertStatementBuilder.Append($"{(item.Region3 == null ? "null," : $"'{item.Region3.Replace("'", "''")}',")}");
                insertStatementBuilder.Append($"{(item.Region4 == null ? "null," : $"'{item.Region4.Replace("'", "''")}',")}");
                insertStatementBuilder.Append($"{(item.Locality == null ? "null," : $"'{item.Locality.Replace("'", "''")}',")}");
                insertStatementBuilder.Append($"{(item.Postcode == null ? "null," : $"'{item.Postcode}',")}");
                insertStatementBuilder.Append($"{(item.Suburb == null ? "null," : $"'{item.Suburb.Replace("'", "''")}',")}");
                insertStatementBuilder.Append($"{(item.Longitude == null ? "null," : $"{item.Longitude},")}");
                insertStatementBuilder.Append($"{(item.Latitude == null ? "null)" : $"{item.Latitude})")}");

                if (index == 5000)
                {
                    insertStatementBuilder.Append(";");
                    await ExecuteSql(connection, insertStatementBuilder.ToString());
                    index = 0;
                }
                else
                {
                    insertStatementBuilder.Append(",");
                    index++;
                }
            }
            if (index > 0)
            {
                insertStatementBuilder.Append(";");
                await ExecuteSql(connection, insertStatementBuilder.ToString());
            }
            await ExecuteSql(connection, $@"
                    CREATE INDEX IX_{tableName}_Postcode ON {tempTableName} (Postcode);
                    CREATE INDEX IX_{tableName}_Locality ON {tempTableName} (Locality);
                    ");

            await ExecuteSql(connection, $@"DROP TABLE IF EXISTS {tableName};");

            await ExecuteSql(connection, $@"RENAME TABLE {tempTableName} TO {tableName};");

        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Change(-1, 0);
            return Task.CompletedTask;
        }
    }
}
