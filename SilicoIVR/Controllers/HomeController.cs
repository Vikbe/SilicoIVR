using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SilicoIVR.Models;

namespace SilicoIVR.Controllers
{
    public class HomeController : Controller
    {
        private SilicoDBContext _context;

        public HomeController(SilicoDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAgent(HomeViewModel model)
        {

            _context.Agents.Add(new Agent
            {
                Extension = model.Extension,
                PhoneNumber = model.PhoneNumber,
                Name = model.Name
            });

            await _context.SaveChangesAsync();


            return Ok();
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
