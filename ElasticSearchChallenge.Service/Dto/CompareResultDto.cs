using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.Service.Dto
{
    public class CompareResultDto<T>
    {
        public List<CompareDetailDto<T>> Details { get; set; }
            = new List<CompareDetailDto<T>>();
    }

    public class CompareDetailDto<T>
    {
        public string Method { get; set; }

        public string Description { get; set; }

        public bool IsSame { get; set; }

        public IEnumerable<T> ElasticSearchData { get; set; }

        public IEnumerable<T> SqlData { get; set; }
    }
}