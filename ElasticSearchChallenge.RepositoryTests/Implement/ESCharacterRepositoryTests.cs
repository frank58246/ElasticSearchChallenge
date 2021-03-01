﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElasticSearchChallenge.Repository.Implement;
using System;
using System.Collections.Generic;
using System.Text;
using ElasticSearchChallenge.RepositoryTests;
using Nest;
using System.Threading.Tasks;
using System.Linq;
using FluentAssertions;

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
            _testElasticSearchHelper = new TestElasticSearchHelper();
            _elasticClient = _testElasticSearchHelper.GetElasticClient();
            _index = "character";
            _testElasticSearchHelper.CreateIndex(_index);
        }

        private ESCharacterRepository GetSystemUnderTest()
        {
            return new ESCharacterRepository(this._elasticClient);
        }

        [TestMethod()]
        public async Task GetAllAsyncTest()
        {
            // Arrange
            var sut = this.GetSystemUnderTest();

            // Act
            var actual = await sut.GetAllAsync();

            // Assert
            actual.Count().Should().Be(0);
        }
    }
}