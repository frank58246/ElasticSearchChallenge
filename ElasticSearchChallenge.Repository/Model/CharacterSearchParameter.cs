using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.Repository.Model
{
    public class CharacterSearchParameter
    {
        public List<string> Family { get; set; } = new List<string>();

        public List<string> Origin { get; set; } = new List<string>();

        public List<string> Name { get; set; } = new List<string>();

        public string Sex { get; set; }

        public float UpAge { get; set; }

        public float DownAge { get; set; }

        public DateTime? UpBirthdate { get; set; }

        public DateTime? DownBirthdate { get; set; }
    }
}