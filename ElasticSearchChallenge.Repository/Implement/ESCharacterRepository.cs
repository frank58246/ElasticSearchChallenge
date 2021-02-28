using ElasticSearchChallenge.Repository.Interface;
using ElasticSearchChallenge.Repository.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchChallenge.Repository.Implement
{
    public class ESCharacterRepository : ICharacterRepository
    {
        public async Task<IEnumerable<Character>> GetAllAsync()
        {
            return Array.Empty<Character>();
        }
    }
}