using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace ElasticSearchChallenge.RepositoryTests
{
    public class TestRelationDatabaseHelper : ITestRelationDatabaseHelper
    {
        public string GetConnectionString(string database)
        {
            var setting = TestHook.DockerSupport.TestSetting;
            var port = setting.ContainerSettings
                              .Where(x => x.ImageName.Contains("mssql"))
                              .FirstOrDefault()
                              .OutterPort;

            var connString = $"Data Source=localhost,{port}; " +
                             $"Initial Catalog={database};" +
                             $"User Id=SA;Password=qazwsx123456!";

            return connString;
        }

        public void CreateDatabase(string database)
        {
            var command = $"CREATE DATABASE {database}";
            Execute("master", command);
        }

        public void DeleteData(string database, string table)
        {
            var command = $"DELETE {table}";
            Execute(database, command);
        }

        public void InsertData(string database, string table)
        {
            var baseDir = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(baseDir, "TestData", $"{table}_Insert.sql");
            var command = File.ReadAllText(filePath);

            Execute(database, command);
        }

        public void CreateTable(string database, string table)
        {
            var baseDir = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(baseDir, "TestData", $"{table}_Create.sql");
            var command = File.ReadAllText(filePath);

            Execute(database, command);
        }

        public void DropTable(string database, string table)
        {
            var command = $"DROP TABLE{table}";

            Execute(database, command);
        }

        public void DropDatabase(string database)
        {
            var command = $"DROP DATABASE{database}";

            Execute("master", command);
        }

        private void Execute(string database, string command)
        {
            var connectionString = GetConnectionString(database);
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = command;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}