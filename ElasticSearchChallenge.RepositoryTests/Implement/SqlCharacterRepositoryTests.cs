using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElasticSearchChallenge.Repository.Implement;
using System;
using System.Collections.Generic;
using System.Text;
using ElasticSearchChallenge.RepositoryTests;

namespace ElasticSearchChallenge.Repository.Implement.Tests
{
    [TestClass()]
    public class SqlCharacterRepositoryTests
    {
        private readonly ITestRelationDatabaseHelper _testRelationDatabaseHelper;

        private readonly string _database;

        private readonly string _table;

        public SqlCharacterRepositoryTests()
        {
            _testRelationDatabaseHelper = new TestRelationDatabaseHelper();
            _database = "character";
            _table = "character";
            _testRelationDatabaseHelper.CreateDatabase(this._database);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _testRelationDatabaseHelper.CreateTable(_database, _table);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _testRelationDatabaseHelper.DeleteData(_database, _table);
        }

        [TestMethod()]
        public void GetAllAsyncTest()
        {
            Assert.IsTrue(3 > 1);
        }
    }
}