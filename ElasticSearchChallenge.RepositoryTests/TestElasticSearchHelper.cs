using CliWrap;
using CliWrap.Buffered;
using ElasticSearchChallenge.Repository.Model;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace ElasticSearchChallenge.RepositoryTests
{
    public class TestElasticSearchHelper : ITestElasticSearchHelper
    {
        private readonly string _elasticUrl;

        private readonly HttpClient _httpClient;

        public TestElasticSearchHelper(HttpClient httpClient, int port)
        {
            this._httpClient = httpClient;
            this._elasticUrl = $"http://localhost:{port}";
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

        public void DeleteData(string index)
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
                                .DefaultIndex("defalt_index")
                                .DisableDirectStreaming(false)
                                .DefaultMappingFor<Character>(m => m
                                .IndexName("character"));

            return new ElasticClient(settings);
        }

        public void InsertData(string index)
        {
            var url = $"{_elasticUrl}/{index}/_bulk?refresh=true";
            var method = "PUT";

            var baseDir = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(baseDir, "TestData", $"{index}_Insert.json");
            var body = File.ReadAllText(filePath);

            Exexcute(method, body, url);
        }

        private void Exexcute(string method, string body, string url)
        {
            HttpResponseMessage response;
            var contentPost = new StringContent(body, Encoding.UTF8, "application/json");

            switch (method)
            {
                case "PUT":
                    response = _httpClient.PutAsync(url, contentPost).GetAwaiter().GetResult();

                    break;

                case "POST":
                    response = _httpClient.PostAsync(url, contentPost).GetAwaiter().GetResult();
                    break;

                default:
                    throw new NotImplementedException(nameof(method));
            }
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                throw new InvalidOperationException(message);
            }
        }
    }
}