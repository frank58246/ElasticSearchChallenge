using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ElasticSearchChallenge.Common.Helper
{
    /// <summary>
    /// 資料庫連線helper
    /// </summary>
    public interface IConnectionHelper
    {
        IDbConnection Character { get; }
    }
}