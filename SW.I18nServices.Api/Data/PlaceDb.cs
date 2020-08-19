using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SW.I18nServices.Api;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SW.I18nService
{
    public class PlaceDb
    {
        private readonly string connectionString;

        public PlaceDb(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString(I18nServicesDbContext.ConnectionString);
        }

        private string SqlStatementGen(string countryCode, string field, string filter, bool distinct = false)
        {
            return SqlStatementGen(countryCode, 
                distinct,
                new List<string> { field }, 
                new Dictionary<string, string> {
                [field] = filter
                }
            );
        }
        private string SqlStatementGen(string countryCode, bool distinct = false, IList<string> fields = null, IDictionary<string, string> filters = null)
        {
            string tableName = $"Places_{countryCode}";
            string sqlStatement = "SELECT " + (distinct? "DISTINCT " : "") + "TOP(1500) ";
            if (fields != null && fields.Count > 0) foreach (string field in fields) sqlStatement += $"[{field.RemoveSpecialCharacters()}],";
            else sqlStatement += "* ";

            //remove last comma
            sqlStatement = sqlStatement.Substring(0, sqlStatement.Length - 1);

            sqlStatement += $" FROM {tableName} ";

            if(filters != null && filters.Count > 0)
            {
                sqlStatement += $"WHERE ";

                int i = 0;
                foreach(var filter in filters)
                {
                    sqlStatement +=  $"{filter.Key.RemoveSpecialCharacters()} LIKE '{filter.Value.RemoveSpecialCharacters()}%'";

                    if (i != (filters.Count - 1)) sqlStatement += " AND ";
                    i++;
                }
            }

            sqlStatement += ';';

            return sqlStatement;
        }


        public async Task<IEnumerable<IDictionary<string, string>>> RetrievePlaces(string countryCode, IList<string> fields, IDictionary<string, string> filters)
        {
            using (MySqlConnection conn = new MySqlConnection { ConnectionString = connectionString })
            {
                conn.Open();
                var rs = await conn.QueryFields(SqlStatementGen(countryCode, false, fields, filters), fields);
                conn.Close();
                return rs;
            }

        }

        public async Task<HashSet<string>> RetrieveLocalities(string countryCode, string query)
        {
            using (MySqlConnection conn = new MySqlConnection { ConnectionString = connectionString })
            {
                conn.Open();
                var rs = await conn.QueryField(SqlStatementGen(countryCode, "Locality", query, true), "Locality");
                conn.Close();
                return rs;
            }

        }
    }
}
