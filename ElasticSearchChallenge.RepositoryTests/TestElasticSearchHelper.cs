using CliWrap;
using CliWrap.Buffered;
using ElasticSearchChallenge.Repository.Model;
using Nest;
using Newtonsoft.Json;
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
            if (this.ExistIndex(index))
            {
                return;
            }

            var url = $"{_elasticUrl}/{index}";
            var method = HttpMethod.Put;

            var baseDir = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(baseDir, "TestData", $"{index}_Create.json");
            var body = File.ReadAllText(filePath);

            this.SendAsync(method, body, url);
        }

        public void DeleteData(string index)
        {
            var url = $"{_elasticUrl}/{index}";
            var method = HttpMethod.Delete;
            var parameter = new
            {
                query = new
                {
                    match_all = new { }
                }
            };
            var body = JsonConvert.SerializeObject(parameter);

            SendAsync(method, body, url);
        }

        public void DeleteIndex(string index)
        {
            var url = $"{_elasticUrl}/{index}";
            var body = "";
            var method = HttpMethod.Delete;

            SendAsync(method, body, url);
        }

        public bool ExistIndex(string index)
        {
            var url = $"{_elasticUrl}/{index}";
            var response = this._httpClient.GetAsync(url).GetAwaiter().GetResult();
            return response.StatusCode.Equals(HttpStatusCode.OK);
        }

        public IElasticClient GetElasticClient()
        {
            var node = new Uri(this._elasticUrl);
            var settings = new ConnectionSettings(node)
                                .DefaultIndex("defalt_index")
                                .DefaultMappingFor<Character>(m => m
                                .IndexName("character"));

            return new ElasticClient(settings);
        }

        public void InsertData(string index)
        {
            var url = $"{_elasticUrl}/{index}/_bulk?refresh=true";
            var method = HttpMethod.Put;

            var baseDir = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(baseDir, "TestData", $"{index}_Insert.json");
            var body = File.ReadAllText(filePath);

            SendAsync(method, body, url);
        }

        private void SendAsync(HttpMethod method, string body, string url)
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json"),
                Method = method,
                RequestUri = new Uri(url)
            };

            var response = this._httpClient.SendAsync(request).GetAwaiter().GetResult();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                throw new InvalidOperationException(message);
            }
        }
    }
}