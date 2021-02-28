using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace ElasticSearchChallenge.Common.Helper
{
    public class ConnectionHelper : IConnectionHelper
    {
        private readonly IDatabaseHelper _databaseHelper;

        public ConnectionHelper(IDatabaseHelper databaseHelper)
        {
            this._databaseHelper = databaseHelper;
        }

        public IDbConnection Character =>
            new SqlConnection(_databaseHelper.Character);
    }
}