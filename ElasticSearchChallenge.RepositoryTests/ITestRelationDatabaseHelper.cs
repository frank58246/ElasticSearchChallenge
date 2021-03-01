using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.RepositoryTests
{
    public interface ITestRelationDatabaseHelper
    {
        void CreateDatabase(string database);

        void InsertData(string database, string table);

        void DeleteData(string database, string table);
    }
}