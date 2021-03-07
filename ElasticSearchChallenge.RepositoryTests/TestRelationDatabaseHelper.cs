using Dapper;
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
        private readonly int _port;

        public TestRelationDatabaseHelper(int port)
        {
            this._port = port;
        }

        public string GetConnectionString(string database)
        {
            var connString = $"Data Source=localhost,{_port}; " +
                             $"Initial Catalog={database};" +
                             $"User Id=SA;Password=qazwsx123456!";

            return connString;
        }

        public void CreateDatabase(string database)
        {
            if (this.ExistDatabase(database))
            {
                return;
            }

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
            if (ExistTable(database, table))
            {
                return;
            }

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

        public bool ExistDatabase(string database)
        {
            var sql = $"select DB_ID('{database}')";
            var connectionString = GetConnectionString("master");
            using (var conn = new SqlConnection(connectionString))
            {
                var result = conn.QueryFirstOrDefault<bool?>(sql);
                return result != null;
            }
        }

        public bool ExistTable(string database, string table)
        {
            var sql = @"IF EXISTS (SELECT 1
                                   FROM INFORMATION_SCHEMA.TABLES
                                   WHERE TABLE_TYPE='BASE TABLE'
                                   AND TABLE_NAME=@TableName)

                        SELECT 1 AS res ELSE SELECT 0 ";

            var parameter = new { TableName = table };

            var connectionString = GetConnectionString(database);
            using (var conn = new SqlConnection(connectionString))
            {
                return conn.QueryFirstOrDefault<bool>(sql, parameter);
            }
        }
    }
}