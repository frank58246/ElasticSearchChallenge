using ElasticSearchChallenge.Repository.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchChallenge.Repository.Interface
{
    public interface IFamilyRepository
    {
        Task<IEnumerable<string>> SearchAsync(FamilySearchParameter parameter);
    }
}