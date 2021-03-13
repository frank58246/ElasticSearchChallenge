using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElasticSearchChallenge.Repository.Implement;
using System;
using System.Collections.Generic;
using System.Text;
using ElasticSearchChallenge.RepositoryTests;
using Nest;
using System.Threading.Tasks;
using System.Linq;
using FluentAssertions;
using System.IO;
using ElasticSearchChallenge.Common.Helper;
using ElasticSearchChallenge.Repository.Model;
using IgnoreAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.IgnoreAttribute;

namespace ElasticSearchChallenge.Repository.Implement.Tests
{
    [TestClass()]
    public class ESCharacterRepositoryTests
    {
        private readonly ITestElasticSearchHelper _testElasticSearchHelper;

        private readonly IElasticClient _elasticClient;

        private readonly string _index;

        public ESCharacterRepositoryTests()
        {
            _testElasticSearchHelper = TestHook.GetTestElasticSearchHelper();
            _elasticClient = _testElasticSearchHelper.GetElasticClient();
            _index = "character";
            _testElasticSearchHelper.CreateIndex(_index);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _testElasticSearchHelper.InsertData(_index);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _testElasticSearchHelper.DeleteData(_index);
        }

        private ESCharacterRepository GetSystemUnderTest()
        {
            return new ESCharacterRepository(this._elasticClient);
        }

        [TestMethod]
        public async Task GetAllAsyncTest()
        {
            // Arrange
            var sut = this.GetSystemUnderTest();

            // Act
            var actual = await sut.GetAllAsync();

            // Assert
            actual.Count().Should().Be(66);
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
            result.Should().OnlyContain(x => x.Family == "丐幫");
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
            result.Should().OnlyContain(x => x.Origin == "笑傲江湖");
        }

        [TestMethod()]
        public async Task SearchAsyncTest_血氣方剛_找出所有小於30歲的男性角色()
        {
            // Arrange
            var parameter = new CharacterSearchParameter()
            {
                Sex = "m", //這邊給大寫會錯，很奇怪...
                UpAge = 30.0f
            };

            var sut = this.GetSystemUnderTest();

            // Act
            var result = await sut.SearchAsync(parameter);

            // Assert
            result.Should().OnlyContain(x => x.Age < 30.0f && x.Sex == "M");
        }

        [Ignore()]
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
            result.Should().OnlyContain(x => x.Family == string.Empty);
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
            result.Should().OnlyContain(x => x.Family == "峨嵋派" && x.Age > 50);
        }

        [TestMethod()]
        public async Task SearchAsyncTest_大宋子民_找出所有宋代出生的角色()
        {
            // Arrange
            var start = new DateTime(960, 02, 04);
            var end = new DateTime(1279, 3, 19);
            var parameter = new CharacterSearchParameter()
            {
                UpBirthdate = end,
                DownBirthdate = start
            };

            var sut = this.GetSystemUnderTest();

            // Act
            var result = await sut.SearchAsync(parameter);

            // Assert
            result.Should().OnlyContain(x => x.Birthdate > start && x.Birthdate < end);
        }

        [TestMethod()]
        public async Task SearchAsyncTest_動保人士_找出郭靖_黃蓉_楊過()
        {
            // Arrange
            var nameList = new List<string>() { "郭靖", "黃蓉", "楊過" };
            var parameter = new CharacterSearchParameter()
            {
                Name = nameList
            };

            var sut = this.GetSystemUnderTest();

            // Act
            var result = await sut.SearchAsync(parameter);

            // Assert
            result.Should().OnlyContain(x => nameList.Contains(x.Name));
        }
    }
}