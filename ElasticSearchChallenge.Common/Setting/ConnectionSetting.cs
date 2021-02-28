using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.Common.Setting
{
    public class ConnectionSetting
    {
        public Dictionary<string, string> Connections { get; set; } =
            new Dictionary<string, string>();
    }
}