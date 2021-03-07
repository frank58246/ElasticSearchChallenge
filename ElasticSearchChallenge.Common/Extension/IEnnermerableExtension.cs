using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElasticSearchChallenge.Common.Extension
{
    public static class IEnnermerableExtension
    {
        public static bool HasValue<T>(this IEnumerable<T> source)
        {
            if (source != null)
            {
                return false;
            }

            return source.Count() > 0;
        }
    }
}