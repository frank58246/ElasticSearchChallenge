using ElasticSearchChallenge.Common.Helper;
using ElasticSearchChallenge.Common.Setting;
using ElasticSearchChallenge.Repository.Implement;
using ElasticSearchChallenge.Repository.Interface;
using ElasticSearchChallenge.Repository.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearchChallenge.Web.Infrastructure.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        public static void AddDependencyInjection(this IServiceCollection services,
          IConfiguration configuration)
        {
            // config
            services.AddConfig(configuration);

            // repository
            services.AddRepository();

            // elastic search
            services.AddElasticSearch();
        }

        private static void AddConfig(this IServiceCollection services,
          IConfiguration configuration)
        {
            var config = new ConnectionSetting();
            configuration.Bind("ConnectionSetting", config);
            services.AddSingleton(config);
        }

        private static void AddRepository(this IServiceCollection services)
        {
            services.AddTransient<ICharacterRepository, SqlCharacterRepository>()
                    .AddTransient<ICharacterRepository, ESCharacterRepository>();
        }

        private static void AddElasticSearch(this IServiceCollection services)
        {
            var databaseHelper = services.BuildServiceProvider()
                                         .GetService<IDatabaseHelper>();

            var uri = new Uri(databaseHelper.ElasticSearch);
            var settings = new ConnectionSettings(uri)
                .DefaultIndex("defaul_tindex")
                            .DefaultMappingFor<Character>(m => m
                                .IndexName("character"));

            var elasticSearchClient = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(elasticSearchClient);
        }
    }
}