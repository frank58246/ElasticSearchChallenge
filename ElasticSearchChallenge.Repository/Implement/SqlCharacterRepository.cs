using Dapper;
using ElasticSearchChallenge.Common.Extension;
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
                conn.Open();
                return await conn.QueryAsync<Character>(sql);
            }
        }

        public async Task<IEnumerable<Character>> SearchAsync(CharacterSearchParameter parameter)
        {
            var sql = @"
                SELECT *
                FROM Character WITH(NOLOCK)
                WHERE 1= 1 ";

            var dynamicParametert = new DynamicParameters();

            if (parameter.Family.HasValue())
            {
                sql += "AND Family IN @Family ";
                dynamicParametert.Add("@Family", parameter.Family);
            }

            if (parameter.Name.HasValue())
            {
                sql += "AND Name IN @Name";
                dynamicParametert.Add("@Name", parameter.Name);
            }

            if (parameter.Origin.HasValue())
            {
                sql += "AND Origin IN @Origin ";
                dynamicParametert.Add("@Origin", parameter.Origin);
            }

            if (parameter.UpBirthdate != null)
            {
                sql += "AND Birthdate < @UpBirthdate ";
                dynamicParametert.Add("@UpBirthdate", parameter.UpBirthdate);
            }

            if (parameter.DownBirthdate != null)
            {
                sql += "AND Birthdate > @DownBirthdate";
                dynamicParametert.Add("@DownBirthdate", parameter.DownBirthdate);
            }

            if (parameter.UpAge > 0)
            {
                sql += "AND Age < @Age ";
                dynamicParametert.Add("@Age", parameter.UpAge);
            }

            if (parameter.DownAge > 0)
            {
                sql += "AND Age > @Age ";
                dynamicParametert.Add("@Age", parameter.DownAge);
            }

            if (parameter.Sex.HasValue())
            {
                sql += "AND Sex = @Sex ";
                dynamicParametert.Add("@Sex", parameter.Sex);
            }

            using (var conn = this._connectionHelper.Character)
            {
                return await conn.QueryAsync<Character>(sql, dynamicParametert);
            }
        }
    }
}