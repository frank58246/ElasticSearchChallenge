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
using ElasticSearchChallenge.Repository.Model;
using System.IO;

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
            // mock
            _testRelationDatabaseHelper = TestHook.GetTestRelationDatabaseHelper();
            _database = "character";
            _table = "character";
            _connectionHelper = Substitute.For<IConnectionHelper>();
            _connectionHelper.Character.
                Returns(new SqlConnection(_testRelationDatabaseHelper.GetConnectionString(_database)));

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

        private SqlCharacterRepository GetSystemUnderTest()
        {
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

        [TestMethod()]
        public async Task SearchAsyncTest_無產階級_找出所有丐幫的角色()
        {
            // Arrange
            var parameter = new CharacterSearchParameter()
            {
                Family = new List<string>() { "丐幫" }
            };

            var sut = this.GetSystemUnderTest();

            // Act
            var result = await sut.SearchAsync(parameter);

            // Assert
            result.All(x => x.Family == "丐幫");
        }

        [TestMethod()]
        public async Task SearchAsyncTest_人在江湖_找出所有笑傲江湖的角色()
        {
            // Arrange
            var parameter = new CharacterSearchParameter()
            {
                Origin = new List<string>() { "笑傲江湖" }
            };

            var sut = this.GetSystemUnderTest();

            // Act
            var result = await sut.SearchAsync(parameter);

            // Assert
            result.All(x => x.Origin == "笑傲江湖");
        }

        [TestMethod()]
        public async Task SearchAsyncTest_血氣方剛_找出所有小於30歲的男性角色()
        {
            // Arrange
            var parameter = new CharacterSearchParameter()
            {
                Sex = "M"
            };

            var sut = this.GetSystemUnderTest();

            // Act
            var result = await sut.SearchAsync(parameter);

            // Assert
            result.All(x => x.Age < 30.0f && x.Sex == "M");
        }

        [TestMethod()]
        public async Task SearchAsyncTest_中間選民_找出所有沒有門派的腳色()
        {
            // Arrange
            var parameter = new CharacterSearchParameter()
            {
                Family = new List<string>() { string.Empty }
            };

            var sut = this.GetSystemUnderTest();

            // Act
            var result = await sut.SearchAsync(parameter);

            // Assert
            result.All(x => x.Family == string.Empty);
        }

        [TestMethod()]
        public async Task SearchAsyncTest_峨嵋耆老_找出所有峨嵋派大於60歲的角色()
        {
            // Arrange
            var parameter = new CharacterSearchParameter()
            {
                Family = new List<string>() { "峨嵋派" },
                DownAge = 50
            };

            var sut = this.GetSystemUnderTest();

            // Act
            var result = await sut.SearchAsync(parameter);

            // Assert
            result.All(x => x.Family == "峨嵋派" && x.Age > 50);
        }

        [TestMethod()]
        public async Task SearchAsyncTest_大宋子民_找出所有宋代出生的角色()
        {
            // Arrange
            var start = new DateTime(960, 02, 04);
            var end = new DateTime(1279, 3, 19);
            var parameter = new CharacterSearchParameter()
            {
                DownBirthdate = start,
                UpBirthdate = end
            };

            var sut = this.GetSystemUnderTest();

            // Act
            var result = await sut.SearchAsync(parameter);

            // Assert
            result.All(x => x.Birthdate > start && x.Birthdate < end);
        }

        [TestMethod()]
        public async Task SearchAsyncTest_動保人士_找出郭靖_黃蓉_楊過()
        {
            // Arrange
            var nameList = new List<string>() { "郭靖", "黃蓉", "楊過" };
            var end = new DateTime(1279, 3, 19);
            var parameter = new CharacterSearchParameter()
            {
                Name = nameList
            };

            var sut = this.GetSystemUnderTest();

            // Act
            var result = await sut.SearchAsync(parameter);

            // Assert
            result.All(x => nameList.Contains(x.Name));
        }
    }
}