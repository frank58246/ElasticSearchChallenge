using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ElasticSearchChallenge.RepositoryTests
{
    [TestClass]
    public class TestHook
    {
        private static DockerSupport _dockerSupport;

        static TestHook()
        {
            _dockerSupport = new DockerSupport();
        }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            // 啟動前先停止container
            // 避免上一次執行的container沒有被正常關閉
            // 導致啟動失敗
            _dockerSupport.StopContainer();
            _dockerSupport.StartContainer();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanUp()
        {
            _dockerSupport.StopContainer();
        }

        public static ITestElasticSearchHelper GetTestElasticSearchHelper()
        {
            var setting = _dockerSupport.TestSetting;
            var port = setting.ContainerSettings
                         .Where(x => x.ImageName.Contains("elasticsearch"))
                         .FirstOrDefault()
                         .OutterPort;
            var httpClient = new HttpClient();

            return new TestElasticSearchHelper(httpClient, port);
        }

        public static ITestRelationDatabaseHelper GetTestRelationDatabaseHelper()
        {
            var setting = _dockerSupport.TestSetting;
            var port = setting.ContainerSettings
                         .Where(x => x.ImageName.Contains("mssql"))
                         .FirstOrDefault()
                         .OutterPort;

            return new TestRelationDatabaseHelper(port);
        }
    }
}