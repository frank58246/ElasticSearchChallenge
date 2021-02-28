using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.Repository.Model
{
    public class Character
    {
        public int SID { get; set; }

        public string Name { get; set; }

        public string Sex { get; set; }

        public string Famil { get; set; }

        public DateTime Birthdate { get; set; }

        public string Origin { get; set; }

        public float Age { get; set; }
    }
}