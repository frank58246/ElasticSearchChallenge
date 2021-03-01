using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace ElasticSearchChallenge.RepositoryTests
{
    public class TestRelationDatabaseHelper : ITestRelationDatabaseHelper
    {
        private string GetConnectionString(string database)
        {
            var port = TestHook.DockerSupport.Port;
            var connString = $"Data Source=localhost,{port}; " +
                             $"Initial Catalog={database};" +
                             $"User Id=SA;Password=qazwsx123456!";
            return connString;
        }

        public void CreateDatabase(string database)
        {
            var connectionString = GetConnectionString("master");
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"CREATE DATABASE {database}";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteData(string database, string table)
        {
            throw new NotImplementedException();
        }

        public void InsertData(string database, string table)
        {
            throw new NotImplementedException();
        }
    }
}