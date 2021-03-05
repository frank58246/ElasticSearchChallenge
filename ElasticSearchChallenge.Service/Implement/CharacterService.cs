using ElasticSearchChallenge.Repository.Implement;
using ElasticSearchChallenge.Repository.Interface;
using ElasticSearchChallenge.Repository.Model;
using ElasticSearchChallenge.Service.Dto;
using ElasticSearchChallenge.Service.Interface;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchChallenge.Service.Implement
{
    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _esCharacterRepository;

        private readonly ICharacterRepository _sqlCharacterRepository;

        public CharacterService(IServiceProvider serviceProvider)
        {
            var services = serviceProvider.GetServices<ICharacterRepository>();

            _sqlCharacterRepository = services
                .First(o => o.GetType() == typeof(SqlCharacterRepository));

            _esCharacterRepository = services.
                First(o => o.GetType() == typeof(ESCharacterRepository));
        }

        public async Task<CompareResultDto<Character>> CompareAsync()
        {
            var compareResultDto = new CompareResultDto<Character>();
            var sqlData = await this._sqlCharacterRepository.GetByFamily("華山派");
            var esData = await this._esCharacterRepository.GetByFamily("華山派");

            var compareDetail = new CompareDetailDto<Character>()
            {
                Method = nameof(ICharacterRepository.GetByFamily),
                Description = "找出所有華山派的人物",
                SqlData = sqlData,
                ElasticSearchData = esData,
                IsSame = IsSame(sqlData, esData)
            };
            compareResultDto.Details.Add(compareDetail);

            return compareResultDto;
        }

        private bool IsSame(IEnumerable<Character> source, IEnumerable<Character> target)
        {
            if (source.Count() != target.Count())
            {
                return false;
            }

            var sourceHashList = source.Select(x => JsonConvert.SerializeObject(x)).ToList();
            sourceHashList.Sort();

            var targetHashList = target.Select(x => JsonConvert.SerializeObject(x)).ToList();
            targetHashList.Sort();

            for (int i = 0; i < sourceHashList.Count(); i++)
            {
                if (sourceHashList[i] != targetHashList[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}