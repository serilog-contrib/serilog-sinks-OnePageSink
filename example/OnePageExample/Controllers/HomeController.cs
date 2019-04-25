using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnePageExample.Models;
using Serilog;
using Serilog.Core;

namespace OnePageExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger logger = Log.ForContext<HomeController>();

        public IActionResult Index()
        {
            logger.Warning("This is a message for the masses. Current time is {CurrentTime}", DateTime.Now.ToString());
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
