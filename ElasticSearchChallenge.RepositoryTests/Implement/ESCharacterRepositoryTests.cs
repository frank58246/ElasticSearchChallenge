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

namespace ElasticSearchChallenge.Repository.Implement.Tests
{
    [TestClass()]
    public class ESCharacterRepositoryTests
    {
        private readonly ITestElasticSearchHelper _testElasticSearchHelper;

        private readonly IElasticClient _elasticClient;

        private readonly IDatabaseHelper _databaseHelper;

        private readonly string _index;

        public ESCharacterRepositoryTests()
        {
            _testElasticSearchHelper = new TestElasticSearchHelper();
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
        }

        private ESCharacterRepository GetSystemUnderTest()
        {
            return new ESCharacterRepository(this._elasticClient);
        }

        private SqlCharacterRepository GetSqlCharacterRepository()
        {
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
    }
}