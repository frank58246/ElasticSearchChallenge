using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.RepositoryTests
{
    public interface ITestRelationDatabaseHelper
    {
        bool ExistDatabase(string database);

        void CreateDatabase(string database);

        void DropDatabase(string database);

        bool ExistTable(string database, string table);

        void CreateTable(string database, string table);

        void DropTable(string database, string table);

        void InsertData(string database, string table);

        void DeleteData(string database, string table);

        string GetConnectionString(string database);
    }
}