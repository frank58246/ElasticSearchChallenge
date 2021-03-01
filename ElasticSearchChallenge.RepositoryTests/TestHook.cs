using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.RepositoryTests
{
    [TestClass]
    public class TestHook
    {
        private static DockerSupport dockerSupport = new DockerSupport();

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            var start = dockerSupport.StartSSMSContainer();

            if (start.Equals(false))
            {
                throw new InvalidOperationException("start container fail");
            }
        }

        [AssemblyCleanup]
        public static void AssemblyCleanUp()
        {
            dockerSupport.StopSSMSContainer();
        }
    }
}