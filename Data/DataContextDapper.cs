using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Data
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

        public bool ExecuteSqlWithParameters(string sql, List<SqlParameter> parameters)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            SqlConnection dbConnection = new(connectionString);

            SqlCommand commandWithParams = new(sql);

            foreach (SqlParameter parameter in parameters)
            {
                commandWithParams.Parameters.Add(parameter);
            }

            dbConnection.Open();

            commandWithParams.Connection = dbConnection;

            int rowsAffected = commandWithParams.ExecuteNonQuery();

            dbConnection.Close();

            return rowsAffected > 0;
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            IDbConnection dbConnection = new SqlConnection(connectionString);

            return dbConnection.Execute(sql);
        }
    }
}

