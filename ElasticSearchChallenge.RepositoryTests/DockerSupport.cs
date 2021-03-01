using CliWrap;
using System;
using System.Collections.Generic;
using System.Text;

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
            var startCommand = $"run -d --rm=true --name {_containerName} -p {Port}:1433 mcr.microsoft.com/mssql/server";
            var cmd = Cli.Wrap("docker")
                      .WithArguments(startCommand)
                      .ExecuteAsync()
                      .GetAwaiter()
                      .GetResult();
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