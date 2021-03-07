using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.Common.Extension
{
    public static class StringExtension
    {
        public static bool HasValue(this string s)
        {
            if (s is null)
            {
                return false;
            }

            return s != string.Empty;
        }
    }
}