using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.Common.Model
{
    public class Result : IResult
    {
        public bool Success { get; set; }

        public int AffectRow { get; set; }

        public string Message { get; set; }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }
    }
}