using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PowerON.DAL;
using PowerON.Infrastructure;
using PowerON.Models;
using PowerON.ViewModel;

namespace PowerON.Controllers
{
    public class HomeController : Controller
    {
        private readonly StoreContext _db;
        private readonly IMemoryCache _cache;

        public HomeController(StoreContext context, IMemoryCache cache)
        {
            _db = context;
            _cache = cache;
        }


        public IActionResult Index()
        {



            if (!_cache.TryGetValue(Const.genrMenuCacheKey, out List<Genre> genrelist))
            {
                if (genrelist == null)
                {
                    genrelist = _db.Genres.ToList();
                }
                _cache.Set(Const.genrMenuCacheKey, genrelist, TimeSpan.FromMinutes(30));
            }


            if (!_cache.TryGetValue(Const.newItemsCacheKey, out List<Item> newItems))
            {
                if (newItems == null)
                {
                    newItems = _db.Items.Where(a => !a.IsHidden).OrderByDescending(a => a.DateAdded).Take(3).ToList();
                }
                _cache.Set(Const.newItemsCacheKey, newItems, TimeSpan.FromMinutes(30));
            }

            var bestsellers = _db.Items.Where(a => !a.IsHidden && a.IsBestseller).OrderBy(g => Guid.NewGuid()).Take(3).ToList();

            var vm = new HomeViewModel()
            {
                Bestsellers = bestsellers,
                NewArrivals = newItems,
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
