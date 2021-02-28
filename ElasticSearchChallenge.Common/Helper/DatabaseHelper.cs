using ElasticSearchChallenge.Common.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.Common.Helper
{
    public class DatabaseHelper : IDatabaseHelper
    {
        private readonly ConnectionSetting _connectionSetting;

        public DatabaseHelper(ConnectionSetting connectionSetting)
        {
            this._connectionSetting = connectionSetting;
        }

        public string Character => this.GetConnectionString("Character");

        public string ElasticSearch => this.GetConnectionString("ElasticSearch");

        private string GetConnectionString(string key)
        {
            if (this._connectionSetting.Connections.ContainsKey(key))
            {
                return this._connectionSetting.Connections[key];
            }
            return "";
        }
    }
}