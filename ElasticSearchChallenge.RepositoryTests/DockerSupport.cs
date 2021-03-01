using CliWrap;
using CliWrap.Buffered;
using ElasticSearchChallenge.RepositoryTests.Setting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ElasticSearchChallenge.RepositoryTests
{
    public class DockerSupport
    {
        private readonly TestSetting _testSetting;

        public TestSetting TestSetting => _testSetting;

        public DockerSupport()
        {
            var baseDir = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(baseDir, "Setting", "TestSetting.json");

            using (var r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                this._testSetting = JsonConvert.DeserializeObject<TestSetting>(json);
            }
        }

        public bool StartContainer()
        {
            var containerSettings = this._testSetting.ContainerSettings;

            // 啟動container
            foreach (var containerSetting in containerSettings)
            {
                Cli.Wrap("docker")
                     .WithArguments(containerSetting.RunCommand)
                     .ExecuteAsync()
                     .GetAwaiter()
                     .GetResult();
            }

            // 檢查服務是否啟用完成
            var isReady = false;
            var timeout = TimeSpan.FromSeconds(30);

            while (isReady.Equals(false))
            {
                isReady = true;
                foreach (var containerSetting in containerSettings)
                {
                    var logs = Cli.Wrap("docker")
                    .WithArguments($"logs {containerSetting.ContainerName}")
                    .ExecuteBufferedAsync()
                    .GetAwaiter()
                    .GetResult();

                    isReady &= logs.StandardOutput.Contains(containerSetting.ReadyMessage);
                }

                // 檢查是否timeout
                var waitSeconds = TimeSpan.FromSeconds(5);
                Thread.Sleep(waitSeconds);
                timeout -= waitSeconds;
                if (timeout.Seconds < 0)
                {
                    throw new TimeoutException();
                }
            }

            return true;
        }

        public void StopContainer()
        {
            var containerSettings = this._testSetting.ContainerSettings;
            foreach (var containerSetting in containerSettings)
            {
                Cli.Wrap("docker")
                    .WithArguments($"stop {containerSetting.ContainerName}")
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteAsync()
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}