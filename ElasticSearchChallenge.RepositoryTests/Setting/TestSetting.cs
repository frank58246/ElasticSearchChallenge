using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.RepositoryTests.Setting
{
    public class TestSetting
    {
        public List<ContainerSetting> ContainerSettings { get; set; }
    }

    public class ContainerSetting
    {
        public string ContainerName { get; set; }

        public string ImageName { get; set; }

        public int OutterPort { get; set; }

        public int InnerPort { get; set; }

        public string Argument { get; set; }

        public string RunCommand =>
         $"run --rm=true -d {Argument} " +
         $"--name {ContainerName} -p {OutterPort}:{InnerPort} {ImageName}";

        public string ReadyMessage { get; set; }
    }
}