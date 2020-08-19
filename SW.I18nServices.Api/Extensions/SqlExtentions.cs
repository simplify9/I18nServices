using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SW.I18nService
{
    static class SqlExtentions
    {
        async public static Task ExecuteSql(this MySqlConnection sqlConnection, string sql)
        {
            using var command = sqlConnection.CreateCommand();
            command.CommandText = sql;
            await command.ExecuteNonQueryAsync();
        }

        async public static Task<HashSet<string>> QueryField(this MySqlConnection sqlConnection, string sql, string field)
        {
            HashSet<string> rs = new HashSet<string>();
            using (MySqlCommand cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandText = sql;
                using(var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                        rs.Add(reader.GetString(reader.GetOrdinal(field)).ToString());
                }
            }
            return rs;
        }

        async public static Task<IEnumerable<IDictionary<string, string>>> QueryFields(this MySqlConnection sqlConnection, string sql, IList<string> fields)
        {
            List<Dictionary<string, string>> rs = new List<Dictionary<string, string>>();
            int i = 0;
            using (MySqlCommand cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandText = sql;
                using(MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, string> row = new Dictionary<string, string>();

                        if(fields.Count == 0)
                        {
                            row["Language"] = reader.GetString(0).ToString();
                            row["Region1"] = reader.GetString(1).ToString();
                            row["Region2"] = reader.GetString(2).ToString();
                            row["Region3"] = reader.GetString(3).ToString();
                            row["Region4"] = reader.GetString(4).ToString();
                            row["Locality"] = reader.GetString(5).ToString();
                            row["Postcode"] = reader.GetString(6).ToString();
                            row["Suburb"] = reader.GetString(7).ToString();
                            row["Longitude"] = reader.GetDecimal(8).ToString();
                            row["Latitude"] = reader.GetDecimal(9).ToString();
                        }
                        else foreach(var field in fields)
                            row[field] = reader.GetString(reader.GetOrdinal(field));
                        rs.Add(row);
                        i++;
                    }
                }
            }
            rs.Insert(0, new Dictionary<string, string> { ["Results"] = i.ToString() });
            return rs;
        }
    }
}
