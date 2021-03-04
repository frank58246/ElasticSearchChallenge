using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ElasticSearchChallenge.RepositoryTests
{
    [TestClass]
    public class TestHook
    {
        public static DockerSupport DockerSupport = new DockerSupport();

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            // 啟動前先停止container
            // 避免上一次執行的container沒有被正常關閉
            // 導致啟動失敗
            DockerSupport.StopContainer();
            DockerSupport.StartContainer();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanUp()
        {
            //DockerSupport.StopContainer();
        }
    }
}