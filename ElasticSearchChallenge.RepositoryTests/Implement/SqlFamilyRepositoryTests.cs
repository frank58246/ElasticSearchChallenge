using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElasticSearchChallenge.Repository.Implement;
using System;
using System.Collections.Generic;
using System.Text;
using ElasticSearchChallenge.RepositoryTests;
using ElasticSearchChallenge.Common.Helper;
using NSubstitute;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ElasticSearchChallenge.Repository.Interface;
using ElasticSearchChallenge.Repository.Model;
using FluentAssertions;
using System.Linq;

namespace ElasticSearchChallenge.Repository.Implement.Tests
{
    [TestClass()]
    public class SqlFamilyRepositoryTests
    {
        private readonly ITestRelationDatabaseHelper _testRelationDatabaseHelper;

        private readonly string _database;

        private readonly string _table;

        private IConnectionHelper _connectionHelper;

        private IConnectionHelper _connectionHelper2;

        public SqlFamilyRepositoryTests()
        {
            // mock
            _testRelationDatabaseHelper = TestHook.GetTestRelationDatabaseHelper();
            _database = "character";
            _table = "character";
            var connString = _testRelationDatabaseHelper.GetConnectionString(_database);
            _connectionHelper = Substitute.For<IConnectionHelper>();
            _connectionHelper.Character.
                Returns(new SqlConnection(connString));

            _connectionHelper2 = Substitute.For<IConnectionHelper>();
            _connectionHelper2.Character.
                Returns(new SqlConnection(connString));

            _testRelationDatabaseHelper.CreateDatabase(this._database);
            _testRelationDatabaseHelper.CreateTable(_database, _table);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _testRelationDatabaseHelper.InsertData(_database, _table);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _testRelationDatabaseHelper.DeleteData(_database, _table);
        }

        private IFamilyRepository GetSystemUnderTest()
        {
            return new SqlFamilyRepository(this._connectionHelper);
        }

        private async Task<IEnumerable<Character>> GetAllCharacterAsync()
        {
            var characterRepository = new SqlCharacterRepository(this._connectionHelper2);
            return await characterRepository.GetAllAsync();
        }

        [TestMethod()]
        public async Task SearchAsyncTest_垂垂老矣_平均年齡大於60歲的幫派()
        {
            // Arrange
            var minAverageAge = 60.0f;
            var parameter = new FamilySearchParameter
            {
                MinAverageAge = minAverageAge
            };
            var sut = this.GetSystemUnderTest();

            // Act
            var result = await sut.SearchAsync(parameter);
            var allCharacter = await this.GetAllCharacterAsync();

            // Assert
            foreach (var family in result)
            {
                var actual = allCharacter.Where(x => x.Family == family);
                actual.Average(x => x.Age).Should().BeGreaterThan(minAverageAge);
            }
        }

        [TestMethod()]
        public async Task SearchAsyncTest_後繼無人_人數少於5人的幫派()
        {
            // Arrange
            var maxMemberCount = 5;
            var parameter = new FamilySearchParameter
            {
                MaxMemberCount = maxMemberCount
            };
            var sut = this.GetSystemUnderTest();

            // Act
            var result = await sut.SearchAsync(parameter);
            var allCharacter = await this.GetAllCharacterAsync();

            // Assert
            foreach (var family in result)
            {
                var actual = allCharacter.Where(x => x.Family == family);
                actual.Count().Should().BeLessThan(maxMemberCount);
            }
        }
    }
}