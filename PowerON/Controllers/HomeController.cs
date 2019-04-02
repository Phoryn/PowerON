using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PowerON.DAL;
using PowerON.Models;

namespace PowerON.Controllers
{
    public class HomeController : Controller
    {
        private readonly StoreContext _context;

        public HomeController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var genrelist = _context.Genres.ToList();
            return View();
        }

        public IActionResult StaticContent(string viewname)
        {

            return View(viewname);
        }
    }
}
