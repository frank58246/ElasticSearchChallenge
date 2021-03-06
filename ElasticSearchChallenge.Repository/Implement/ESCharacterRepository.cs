using ElasticSearchChallenge.Common.Model;
using ElasticSearchChallenge.Repository.Interface;
using ElasticSearchChallenge.Repository.Model;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchChallenge.Repository.Implement
{
    public class ESCharacterRepository : ICharacterRepository
    {
        private readonly IElasticClient _elasticClient;

        public ESCharacterRepository(IElasticClient elasticClient)
        {
            this._elasticClient = elasticClient;
        }

        public async Task<IEnumerable<Character>> GetAllAsync()
        {
            var searchRequest = new SearchRequest("character")
            {
                Size = 100,
                Query = new MatchAllQuery()
            };

            var response = await this._elasticClient.SearchAsync<Character>(searchRequest);

            return response.Documents;
        }

        public async Task<IEnumerable<Character>> GetByFamily(string family)
        {
            var searchRequest = new SearchRequest("character")
            {
                Size = 100,
                Query = new TermQuery
                {
                    Field = Infer.Field<Character>(c => c.Family),
                    Value = family
                }
            };

            var response = await this._elasticClient.SearchAsync<Character>(searchRequest);

            return response.Documents;
        }

        public async Task<IEnumerable<Character>> SearchAsync(CharacterSearchParameter parameter)
        {
            throw new NotImplementedException();
        }
    }
}