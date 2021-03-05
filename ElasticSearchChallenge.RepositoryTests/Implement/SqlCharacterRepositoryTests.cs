using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElasticSearchChallenge.Repository.Implement;
using System;
using System.Collections.Generic;
using System.Text;
using ElasticSearchChallenge.RepositoryTests;
using ElasticSearchChallenge.Common.Helper;
using System.Threading.Tasks;
using System.Linq;
using NSubstitute;
using System.Data.SqlClient;
using FluentAssertions;

namespace ElasticSearchChallenge.Repository.Implement.Tests
{
    [TestClass()]
    public class SqlCharacterRepositoryTests
    {
        private readonly ITestRelationDatabaseHelper _testRelationDatabaseHelper;

        private readonly string _database;

        private readonly string _table;

        private IConnectionHelper _connectionHelper;

        public SqlCharacterRepositoryTests()
        {
            _testRelationDatabaseHelper = TestHook.GetTestRelationDatabaseHelper();
            _database = "character";
            _table = "character";
            _testRelationDatabaseHelper.CreateDatabase(this._database);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _testRelationDatabaseHelper.CreateTable(_database, _table);
            _testRelationDatabaseHelper.InsertData(_database, _table);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _testRelationDatabaseHelper.DeleteData(_database, _table);
        }

        private SqlCharacterRepository GetSystemUnderTest()
        {
            this._connectionHelper = Substitute.For<IConnectionHelper>();
            var connectionString = _testRelationDatabaseHelper.GetConnectionString(_database);
            this._connectionHelper.Character.Returns(new SqlConnection(connectionString));

            return new SqlCharacterRepository(this._connectionHelper);
        }

        [TestMethod()]
        public async Task GetAllAsyncTest()
        {
            // Arrange
            var sut = this.GetSystemUnderTest();

            // Act
            var result = await sut.GetAllAsync();

            // Assert
            result.Count().Should().Be(66);
        }
    }
}