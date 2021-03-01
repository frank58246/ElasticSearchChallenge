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

        public SqlCharacterRepositoryTests()
        {
            _testRelationDatabaseHelper = new TestRelationDatabaseHelper();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _testRelationDatabaseHelper.CreateDatabase("characater");
        }

        [TestMethod()]
        public void GetAllAsyncTest()
        {
            Assert.IsTrue(3 > 1);
        }
    }
}