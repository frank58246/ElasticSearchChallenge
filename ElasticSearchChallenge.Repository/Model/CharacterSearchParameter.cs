using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.Repository.Model
{
    public class CharacterSearchParameter
    {
        /// <summary>
        /// 門派
        /// </summary>
        public List<string> Factions { get; set; } = new List<string>();

        /// <summary>
        /// 原著小說
        /// </summary>
        public List<string> Novels { get; set; } = new List<string>();

        /// <summary>
        /// 姓名
        /// </summary>
        public List<string> Names { get; set; } = new List<string>();

        /// <summary>
        /// 姓
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 年紀上限
        /// </summary>
        public float UpAge { get; set; }

        /// <summary>
        /// 年紀下限
        /// </summary>
        public float DownAge { get; set; }

        /// <summary>
        /// 生日上限
        /// </summary>
        public DateTime? UpBirthday { get; set; }

        /// <summary>
        /// 生日下限
        /// </summary>
        public DateTime? DownBirthday { get; set; }

        /// <summary>
        /// 指定的小說以及門派
        /// </summary>
        public List<TargetParameter> Targets { get; set; } = new List<TargetParameter>();
    }/// <summary>

     /// 指定門派和小說的參數
     /// </summary>
    public class TargetParameter
    {
        public TargetParameter()
        {
        }

        public TargetParameter(string novel, string faction)
        {
            this.Novel = novel;
            this.Faction = faction;
        }

        /// <summary>
        /// 原著小說
        /// </summary>
        public string Novel { get; set; }

        /// <summary>
        /// 門派
        /// </summary>
        public string Faction { get; set; }
    }
}