using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.Repository.Model
{
    /// <summary>
    /// 幫派搜尋條件的參數
    /// </summary>
    public class FamilySearchParameter
    {
        /// <summary>
        /// 最大幫派人數
        /// </summary>
        public int MaxMemberCount { get; set; }

        /// <summary>
        /// 最少幫派人數
        /// </summary>
        public int MinMemberCount { get; set; }

        /// <summary>
        /// 最大幫派平均年齡
        /// </summary>
        public float MaxAverageAge { get; set; }

        /// <summary>
        /// 最小幫派平均年齡
        /// </summary>
        public float MinAverageAge { get; set; }

        /// <summary>
        /// 性別限制
        /// </summary>
        public string Sex { get; set; }
    }
}