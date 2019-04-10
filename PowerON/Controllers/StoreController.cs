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
            var genres = _db.Genres.Include("Items").Where(g => g.Name.ToUpper() == genrename.ToUpper()).Single();
            var albums = genres.Items.ToList();
            return View(albums);
        }




    }
}