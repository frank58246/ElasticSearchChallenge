using CliWrap;
using CliWrap.Buffered;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ElasticSearchChallenge.RepositoryTests
{
    public class DockerSupport
    {
        private string _containerName = "SSMS_Unit_Test";

        private string _imageName = "mcr.microsoft.com/mssql/server";

        private int _port = 0;

        public int Port
        {
            get
            {
                if (_port == 0)
                {
                    _port = new Random().Next(44567, 56418);
                }
                return _port;
            }
        }

        // 啟動ssms contaier
        public bool StartSSMSContainer()
        {
            // 啟動container
            var startCommand = $"run -d --rm=true --name {_containerName} -p {Port}:1433  -e SA_PASSWORD=qazwsx123456! -e ACCEPT_EULA=y mcr.microsoft.com/mssql/server";
            Cli.Wrap("docker")
                      .WithArguments(startCommand)
                      .ExecuteAsync()
                      .GetAwaiter()
                      .GetResult();

            // 檢查ssms是否啟動完成
            var logs = Cli.Wrap("docker")
                      .WithArguments($"logs {_containerName}")
                      .ExecuteBufferedAsync()
                      .GetAwaiter()
                      .GetResult();

            var timeout = TimeSpan.FromSeconds(30);
            var readyMessage = "The default language (LCID 0) has been set for engine and full-text services.";
            while (logs.StandardOutput.Contains(readyMessage).Equals(false))
            {
                if (logs.StandardError != string.Empty)
                {
                    throw new InvalidOperationException(logs.StandardError);
                }

                if (timeout.Seconds < 0)
                {
                    throw new TimeoutException("start ssms container timeout");
                }

                var waitSecond = TimeSpan.FromSeconds(3);
                Thread.Sleep(waitSecond);
                timeout -= waitSecond;

                logs = Cli.Wrap("docker")
                   .WithArguments($"logs {_containerName}")
                   .ExecuteBufferedAsync()
                   .GetAwaiter()
                   .GetResult();
            }
            return true;
        }

        public void StopSSMSContainer()
        {
            Cli.Wrap("docker")
               .WithArguments($"stop {_containerName} ")
               .ExecuteAsync()
               .GetAwaiter()
               .GetResult();
        }
    }
}