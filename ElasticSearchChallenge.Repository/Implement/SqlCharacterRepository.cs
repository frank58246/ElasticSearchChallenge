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
                sql += "AND Faction IN @Factions ";
                dynamicParametert.Add("@Factions", parameter.Factions);
            }

            if (parameter.Names.HasValue())
            {
                sql += "AND Name IN @Name";
                dynamicParametert.Add("@Name", parameter.Names);
            }

            if (parameter.LastName.HasValue())
            {
                sql += $"AND Name LIKE N'{parameter.LastName}%' ";
            }
            if (parameter.Novels.HasValue())
            {
                sql += "AND Novel IN @Novels ";
                dynamicParametert.Add("@Novels", parameter.Novels);
            }

            if (parameter.UpBirthday != null)
            {
                // 參數若小於1753年，必須指定型別為Datetime2
                sql += "AND Birthday < @UpBirthday  ";
                dynamicParametert.Add
                (
                     name: "@UpBirthday",
                     parameter.UpBirthday,
                     dbType: System.Data.DbType.DateTime2
                );
            }

            if (parameter.DownBirthday != null)
            {
                // 參數若小於1753年，必須指定型別為Datetime2
                sql += "AND Birthday > @DownBirthday ";
                dynamicParametert.Add
                (
                     name: "@DownBirthday",
                     parameter.DownBirthday,
                     dbType: System.Data.DbType.DateTime2
                );
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
            if (parameter.Targets.HasValue())
            {
                int index = 0;
                var conditonList = new List<string>();
                foreach (var target in parameter.Targets)
                {
                    // 轉為 (Novel= @Novel0 AND Faction = @Faction0)

                    var condition = $"(Novel = @Novel{index} AND " +
                                    $" Faction = @Faction{index} )";
                    conditonList.Add(condition);

                    dynamicParametert.Add($"@Novel{index}", target.Novel);
                    dynamicParametert.Add($"@Faction{index}", target.Faction);
                    index += 1;
                }

                // 加上 AND (condtion1 OR condtion2 Or condtion3 ... )

                var targetClause = string.Join("OR", conditonList);
                sql += $"AND ({targetClause}) ";
            }

            using (var conn = this._connectionHelper.Character)
            {
                return await conn.QueryAsync<Character>(sql, dynamicParametert);
            }
        }
    }
}