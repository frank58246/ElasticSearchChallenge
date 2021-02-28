using ElasticSearchChallenge.Repository.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchChallenge.Repository.Interface
{
    public interface ICharacterRepository
    {
        Task<IEnumerable<Character>> GetAllAsync();
    }
}