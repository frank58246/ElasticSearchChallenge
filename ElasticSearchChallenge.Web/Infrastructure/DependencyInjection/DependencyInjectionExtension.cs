using ElasticSearchChallenge.Common.Setting;
using ElasticSearchChallenge.Repository.Implement;
using ElasticSearchChallenge.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            var config = new ConnectionSetting();
            configuration.Bind("ConnectionSetting", config);
            services.AddSingleton(config);

            // repository
            services.AddTransient<ICharacterRepository, SqlCharacterRepository>()
                    .AddTransient<ICharacterRepository, ESCharacterRepository>();
        }
    }
}