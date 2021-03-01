using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.Common.Model
{
    public interface IResult
    {
        bool Success { get; set; }

        string Message { get; set; }

        int AffectRow { get; set; }
    }
}