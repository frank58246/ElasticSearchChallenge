using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.Common.Helper
{
    /// <summary>
    /// 資料庫連線字串helper
    /// </summary>
    public interface IDatabaseHelper
    {
        string Character { get; }

        string ElasticSearch { get; }
    }
}