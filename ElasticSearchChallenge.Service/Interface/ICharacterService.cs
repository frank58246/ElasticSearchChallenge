using ElasticSearchChallenge.Repository.Model;
using ElasticSearchChallenge.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchChallenge.Service.Interface
{
    public interface ICharacterService
    {
        Task<CompareResultDto<Character>> CompareAsync();
    }
}