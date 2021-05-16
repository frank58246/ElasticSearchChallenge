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

            if (parameter.Factions.HasValue())
            {
                sql += "AND Faction IN @Faction ";
                dynamicParametert.Add("@Faction", parameter.Factions);
            }

            if (parameter.Names.HasValue())
            {
                sql += "AND Name IN @Name";
                dynamicParametert.Add("@Name", parameter.Names);
            }

            if (parameter.Novels.HasValue())
            {
                sql += "AND Novel IN @Novels ";
                dynamicParametert.Add("@Novels", parameter.Novels);
            }

            if (parameter.UpBirthday != null)
            {
                sql += "AND Birthdate < @UpBirthdate ";
                dynamicParametert.Add("@UpBirthdate", parameter.UpBirthday);
            }

            if (parameter.DownBirthday != null)
            {
                sql += "AND Birthdate > @DownBirthdate";
                dynamicParametert.Add("@DownBirthdate", parameter.DownBirthday);
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