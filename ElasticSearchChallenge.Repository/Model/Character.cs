using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.Repository.Model
{
    public class Character
    {
        public int SID { get; set; }

        public string Name { get; set; }

        [Keyword(Name = "sex")]
        public string Sex { get; set; }

        [Keyword]
        public string Faction { get; set; }

        public DateTime Birthday { get; set; }

        [Keyword]
        public string Novel { get; set; }

        public float Age { get; set; }
    }
}