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

        public async Task<IEnumerable<Character>> SearchAsync(CharacterSearchParameter parameter)
        {
            var mustClauses = new List<QueryContainer>();

            var shouldCluause = new List<QueryContainer>();

            if (parameter.Names.HasValue())
            {
                mustClauses.Add(new TermsQuery
                {
                    Field = Infer.Field<Character>(c => c.Name.Suffix("keyword")),
                    Terms = parameter.Names
                });
            }

            if (parameter.LastName.HasValue())
            {
                mustClauses.Add(new MatchPhrasePrefixQuery
                {
                    Field = Infer.Field<Character>(c => c.Name),
                    Query = parameter.LastName
                });
            }
            if (parameter.Factions.HasValue())
            {
                mustClauses.Add(new TermsQuery
                {
                    // IsVerbatim預設為false，在轉換為查詢的json時，
                    // 會忽略沒有用的條件，例如包含空字串的陣列
                    // 導致查詢錯誤

                    IsVerbatim = true,
                    Field = Infer.Field<Character>(c => c.Faction),
                    Terms = parameter.Factions
                });
            }

            if (parameter.Novels.HasValue())
            {
                mustClauses.Add(new TermsQuery
                {
                    Field = Infer.Field<Character>(c => c.Novel),
                    Terms = parameter.Novels
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
            if (parameter.UpBirthday != null)
            {
                mustClauses.Add(new DateRangeQuery
                {
                    Field = Infer.Field<Character>(c => c.Birthday),
                    LessThanOrEqualTo = parameter.UpBirthday
                });
            }

            if (parameter.DownBirthday != null)
            {
                mustClauses.Add(new DateRangeQuery
                {
                    Field = Infer.Field<Character>(c => c.Birthday),
                    GreaterThanOrEqualTo = parameter.DownBirthday
                });
            }

            if (parameter.Sex.HasValue())
            {
                mustClauses.Add(new TermQuery()
                {
                    Field = Infer.Field<Character>(c => c.Sex),
                    Value = parameter.Sex
                });
            }

            if (parameter.Targets.HasValue())
            {
                foreach (var target in parameter.Targets)
                {
                    var novelQuery = new TermQuery()
                    {
                        Field = Infer.Field<Character>(c => c.Novel),
                        Value = target.Novel
                    };

                    var factionQuery = new TermQuery()
                    {
                        Field = Infer.Field<Character>(c => c.Faction),
                        Value = target.Faction
                    };

                    shouldCluause.Add(novelQuery && factionQuery);
                }
            }
            var searchRequest = new SearchRequest("character")
            {
                Size = 100,
                Query = new BoolQuery
                {
                    Must = mustClauses,
                    Should = shouldCluause
                },
            };
            var response = await this._elasticClient.SearchAsync<Character>(searchRequest);

            return response.Documents;
        }
    }
}