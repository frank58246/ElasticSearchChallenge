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
using ElasticSearchChallenge.Service.Interface;
using Newtonsoft.Json;
using ElasticSearchChallenge.Repository.Model;

namespace ElasticSearchChallenge.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICharacterRepository _sqlCharacterRepository;

        private readonly ICharacterRepository _esCharacterRepository;

        private readonly ICharacterService _characterService;

        private readonly ILogger<HomeController> _logger;

        public HomeController(IServiceProvider serviceProvider, ILogger<HomeController> logger)
        {
            var services = serviceProvider.GetServices<ICharacterRepository>();

            _sqlCharacterRepository = services.First(o => o.GetType() == typeof(SqlCharacterRepository));
            _esCharacterRepository = services.First(o => o.GetType() == typeof(ESCharacterRepository));
            _characterService = serviceProvider.GetServices<ICharacterService>().FirstOrDefault();
            _logger = logger;
        }

        public IActionResult Index()
        {
            var a = _sqlCharacterRepository.GetByFamily("華山派").GetAwaiter().GetResult();
            var b = _esCharacterRepository.GetByFamily("華山派").GetAwaiter().GetResult();
            var compare = this._characterService.CompareAsync().GetAwaiter().GetResult();

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