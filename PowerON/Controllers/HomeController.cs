using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PowerON.DAL;
using PowerON.Models;
using PowerON.ViewModel;

namespace PowerON.Controllers
{
    public class HomeController : Controller
    {
        private readonly StoreContext _db;

        public HomeController(StoreContext context)
        {
            _db = context;
        }


        public IActionResult Index()
        {

            var genrelist = _db.Genres.ToList();
            var newArrivals = _db.Items.Where(a => !a.IsHidden).OrderByDescending(a => a.DateAdded).Take(3).ToList();
            var bestsellers = _db.Items.Where(a => !a.IsHidden && a.IsBestseller).OrderBy(g => Guid.NewGuid()).Take(3).ToList();

            var vm = new HomeViewModel()
            {
                Bestsellers = bestsellers,
                NewArrivals = newArrivals,
                Genres = genrelist
            };





            return View(vm);
        }

        public IActionResult StaticContent(string viewname)
        {

            return View(viewname);
        }
    }
}
