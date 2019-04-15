using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerON.DAL;

namespace PowerON.Controllers
{
    public class StoreController : Controller
    {
        private readonly StoreContext _db;

        public StoreController(StoreContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            var album = _db.Items.Find(id);
            return View(album);
        }
        public IActionResult List(string genrename)
        {
            var genre = _db.Genres.Include("Items").Where(g => g.Name.ToUpper() == genrename.ToUpper()).Single();
            return View(genre);
        }

        public IActionResult SearchList(string searchQuery)
        {
            var items = _db.Items.Where(a => (searchQuery == null ||
                                            a.ItemName.ToLower().Contains(searchQuery.ToLower())) 
                                            && !a.IsHidden);

            return PartialView("_ProductList", items);
            //return ViewComponent("ProductList", items);
        }

        public IActionResult AlbumsSuggestions(string term)
        {
            var albums = this._db.Items.Where(a => !a.IsHidden && a.ItemName.ToLower().Contains(term.ToLower()))
                .Take(5).Select(a => new { label = a.ItemName });

            return Json(albums);
        }

    }
}