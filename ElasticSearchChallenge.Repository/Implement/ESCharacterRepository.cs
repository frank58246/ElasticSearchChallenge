using ElasticSearchChallenge.Common.Extension;
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
            var mustClauses = new List<QueryContainer>();

            if (parameter.Name.HasValue())
            {
                mustClauses.Add(new TermQuery
                {
                    Field = Infer.Field<Character>(c => c.Name),
                    Value = parameter.Name
                });
            }

            if (parameter.Family.HasValue())
            {
                mustClauses.Add(new TermQuery
                {
                    Field = Infer.Field<Character>(c => c.Family),
                    Value = parameter.Family
                });
            }

            if (parameter.Origin.HasValue())
            {
                mustClauses.Add(new TermQuery
                {
                    Field = Infer.Field<Character>(c => c.Origin),
                    Value = parameter.Origin
                });
            }

            if (parameter.UpAge > 0)
            {
                mustClauses.Add(new NumericRangeQuery
                {
                    Field = Infer.Field<Character>(c => c.Age),
                    LessThanOrEqualTo = parameter.UpAge
                });
            }

            if (parameter.DownAge > 0)
            {
                mustClauses.Add(new NumericRangeQuery
                {
                    Field = Infer.Field<Character>(c => c.Age),
                    GreaterThanOrEqualTo = parameter.DownAge
                });
            }

            var searchRequest = new SearchRequest("character")
            {
                Size = 100,
                Query = new BoolQuery { Must = mustClauses }
            };
            var response = await this._elasticClient.SearchAsync<Character>(searchRequest);

            return response.Documents;
        }
    }
}