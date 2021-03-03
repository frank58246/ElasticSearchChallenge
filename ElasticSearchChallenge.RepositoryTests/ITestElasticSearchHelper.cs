using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchChallenge.RepositoryTests
{
    public interface ITestElasticSearchHelper
    {
        IElasticClient GetElasticClient();

        void CreateIndex(string index);

        void InsertData(string index);

        void DeleteData(string index);

        void DeleteIndex(string index);
    }
}