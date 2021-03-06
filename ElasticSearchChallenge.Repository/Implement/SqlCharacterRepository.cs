using Dapper;
using ElasticSearchChallenge.Common.Helper;
using ElasticSearchChallenge.Repository.Interface;
using ElasticSearchChallenge.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchChallenge.Repository.Implement
{
    public class SqlCharacterRepository : ICharacterRepository
    {
        private readonly IConnectionHelper _connectionHelper;

        public SqlCharacterRepository(IConnectionHelper connectionHelper)
        {
            this._connectionHelper = connectionHelper;
        }

        public async Task<IEnumerable<Character>> GetAllAsync()
        {
            var sql = @"
                SELECT *
                FROM Character WITH(NOLOCK)";

            using (var conn = this._connectionHelper.Character)
            {
                return await conn.QueryAsync<Character>(sql);
            }
        }

        public async Task<IEnumerable<Character>> GetByFamily(string family)
        {
            var sql = @"
                SELECT *
                FROM Character WITH(NOLOCK)
                WHERE Family = @Family";

            using (var conn = this._connectionHelper.Character)
            {
                var parameter = new { Family = family };
                return await conn.QueryAsync<Character>(sql, parameter);
            }
        }

        public async Task<IEnumerable<Character>> SearchAsync(CharacterSearchParameter parameter)
        {
            var sql = @"
                SELECT *
                FROM Character WITH(NOLOCK)
                WHERE 1= 1 ";

            var dynamicParametert = new DynamicParameters();

            if (parameter.Family.Count() > 0)
            {
                sql += "AND Family in @Family";
                dynamicParametert.Add("@Family", parameter.Family);
            }

            using (var conn = this._connectionHelper.Character)
            {
                return await conn.QueryAsync<Character>(sql, dynamicParametert);
            }
        }
    }
}