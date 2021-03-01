using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.Repository.Model
{
    public class Character
    {
        public int SID { get; set; }

        [Keyword]
        public string Name { get; set; }

        public string Sex { get; set; }

        [Keyword]
        public string Family { get; set; }

        public DateTime Birthdate { get; set; }

        [Keyword]
        public string Origin { get; set; }

        public float Age { get; set; }
    }
}