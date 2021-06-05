using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace TPOP_Server.Database
{
    public static class SqlDataAccess
    {
        public static string GetConnectionString(string connectionName = "Default")
        {
            Debug.WriteLine(ConfigurationManager.ConnectionStrings[connectionName].ConnectionString);
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }
        public static async Task<List<T>> LoadData<T>(string sql)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetConnectionString()))
            {
                var list = await cnn.QueryAsync<T>(sql);
                return list.ToList();
            }
        }
        public static async Task<int> SaveData<T>(string sql, T data)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetConnectionString()))
            {
                return await cnn.ExecuteAsync(sql, data);
            }
        }
        public static async Task<int> Execute(string sql)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetConnectionString()))
            {
                return await cnn.ExecuteAsync(sql);
            }
        }
    }
}
