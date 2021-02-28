using ElasticSearchChallenge.Repository.Interface;
using ElasticSearchChallenge.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ElasticSearchChallenge.Repository.Implement;

namespace ElasticSearchChallenge.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICharacterRepository _sqlCharacterRepository;

        private readonly ICharacterRepository _esCharacterRepository;

        private readonly ILogger<HomeController> _logger;

        public HomeController(IServiceProvider serviceProvider, ILogger<HomeController> logger)
        {
            var services = serviceProvider.GetServices<ICharacterRepository>();

            _sqlCharacterRepository = services.First(o => o.GetType() == typeof(SqlCharacterRepository));
            _esCharacterRepository = services.First(o => o.GetType() == typeof(ESCharacterRepository));
            _logger = logger;
        }

        public IActionResult Index()
        {
            var a = _sqlCharacterRepository.GetAllAsync().GetAwaiter().GetResult();
            var b = _esCharacterRepository.GetAllAsync().GetAwaiter().GetResult();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}