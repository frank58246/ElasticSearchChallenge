using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
            var start = DockerSupport.StartSSMSContainer();

            if (start.Equals(false))
            {
                throw new InvalidOperationException("start container fail");
            }
        }

        [AssemblyCleanup]
        public static void AssemblyCleanUp()
        {
            DockerSupport.StopSSMSContainer();
        }
    }
}