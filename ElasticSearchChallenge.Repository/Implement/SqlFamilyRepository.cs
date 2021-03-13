using Dapper;
using ElasticSearchChallenge.Common.Helper;
using ElasticSearchChallenge.Repository.Interface;
using ElasticSearchChallenge.Repository.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchChallenge.Repository.Implement
{
    public class SqlFamilyRepository : IFamilyRepository
    {
        private readonly IConnectionHelper _connectionHelper;

        public SqlFamilyRepository(IConnectionHelper connectionHelper)
        {
            this._connectionHelper = connectionHelper;
        }

        public async Task<IEnumerable<string>> SearchAsync(FamilySearchParameter parameter)
        {
            var whereClause = @" WHERE 1 = 1 ";
            var havingCluase = @"HAVING 1 = 1 ";

            var dynamicParametert = new DynamicParameters();

            if (parameter.MaxMemberCount > 0)
            {
                havingCluase += $"AND COUNT(Name) < @MaxMemberCount ";
                dynamicParametert.Add("@MaxMemberCount", parameter.MaxMemberCount);
            }

            if (parameter.MinAverageAge > 0)
            {
                havingCluase += $"AND AVG(Age) > @MinAverageAge ";
                dynamicParametert.Add("@MinAverageAge", parameter.MinAverageAge);
            }

            var sql = $"SELECT Family FROM character WITH(NOLOCK) " +
                   $"{whereClause} " +
                   $"GROUP BY Family " +
                   $"{havingCluase}" +
                   $"ORDER BY Family ";

            using (var conn = this._connectionHelper.Character)
            {
                return await conn.QueryAsync<string>(sql, dynamicParametert);
            }
        }
    }
}