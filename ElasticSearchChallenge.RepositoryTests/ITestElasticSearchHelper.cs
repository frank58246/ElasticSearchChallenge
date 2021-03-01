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

        void InsertData(string index, string indice);

        void DeleteData(string index, string indice);

        void DeleteIndex(string index);
    }
}