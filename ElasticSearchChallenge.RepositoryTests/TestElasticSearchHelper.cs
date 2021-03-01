using CliWrap;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ElasticSearchChallenge.RepositoryTests
{
    public class TestElasticSearchHelper : ITestElasticSearchHelper
    {
        private readonly string _elasticUrl;

        public TestElasticSearchHelper()
        {
            var setting = TestHook.DockerSupport.TestSetting;
            var port = setting.ContainerSettings
                              .Where(x => x.ImageName.Contains("elasticsearch"))
                              .FirstOrDefault()
                              .OutterPort;

            _elasticUrl = $"http://localhost:{port}";
        }

        public void CreateIndex(string index)
        {
            var url = $"{_elasticUrl}/{index}";
            var method = "PUT";

            var baseDir = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(baseDir, "TestData", $"{index}_Create.json");
            var body = File.ReadAllText(filePath);

            this.Exexcute(method, body, url);
        }

        public void DeleteData(string index, string indice)
        {
            throw new NotImplementedException();
        }

        public void DeleteIndex(string index)
        {
            throw new NotImplementedException();
        }

        public IElasticClient GetElasticClient()
        {
            var node = new Uri(this._elasticUrl);
            var settings = new ConnectionSettings(node)
                                .DefaultIndex("defalt_index");

            return new ElasticClient(settings);
        }

        public void InsertData(string index, string indice)
        {
            throw new NotImplementedException();
        }

        private void Exexcute(string method, string body, string url)
        {
            var argument = $"-X {method} " +
                           $"-H \"Content-Type:application/json\" " +
                           $"-d '{body}' \"{url}\"";

            Cli.Wrap("curl")
               .WithArguments(argument)
               .ExecuteAsync()
               .GetAwaiter()
               .GetResult();
        }
    }
}