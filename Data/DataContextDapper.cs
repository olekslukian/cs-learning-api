using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotnetAPI
{
    public class DataContextDapper(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;


        public IEnumerable<T> LoadData<T>(string sql)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            IDbConnection dbConnection = new SqlConnection(connectionString);

            return dbConnection.Query<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            IDbConnection dbConnection = new SqlConnection(connectionString);

            return dbConnection.QuerySingle<T>(sql);
        }

        public bool ExecuteSql(string sql)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            IDbConnection dbConnection = new SqlConnection(connectionString);

            return dbConnection.Execute(sql) > 0;
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            IDbConnection dbConnection = new SqlConnection(connectionString);

            return dbConnection.Execute(sql);
        }
    }
}

