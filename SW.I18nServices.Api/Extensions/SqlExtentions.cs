using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SW.I18nService
{
    static class SqlExtentions
    {
        async public static Task ExecuteSql(this SqlConnection sqlConnection, string sql)
        {
            using var command = sqlConnection.CreateCommand();
            command.CommandText = sql;
            await command.ExecuteNonQueryAsync();
        }

        async public static Task<HashSet<string>> QueryField(this SqlConnection sqlConnection, string sql, string field)
        {
            HashSet<string> rs = new HashSet<string>();
            using (SqlCommand cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandText = sql;
                using(SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                        rs.Add(reader.GetSqlString(reader.GetOrdinal(field)).ToString());
                }
            }
            return rs;
        }

        async public static Task<IEnumerable<IDictionary<string, string>>> QueryFields(this SqlConnection sqlConnection, string sql, IList<string> fields)
        {
            List<Dictionary<string, string>> rs = new List<Dictionary<string, string>>();
            int i = 0;
            using (SqlCommand cmd = sqlConnection.CreateCommand())
            {
                cmd.CommandText = sql;
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, string> row = new Dictionary<string, string>();

                        if(fields.Count == 0)
                        {
                            row["Language"] = reader.GetSqlString(0).ToString();
                            row["Region1"] = reader.GetSqlString(1).ToString();
                            row["Region2"] = reader.GetSqlString(2).ToString();
                            row["Region3"] = reader.GetSqlString(3).ToString();
                            row["Region4"] = reader.GetSqlString(4).ToString();
                            row["Locality"] = reader.GetSqlString(5).ToString();
                            row["Postcode"] = reader.GetSqlString(6).ToString();
                            row["Suburb"] = reader.GetSqlString(7).ToString();
                            row["Longitude"] = reader.GetSqlDecimal(8).ToString();
                            row["Latitude"] = reader.GetSqlDecimal(9).ToString();
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
