using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerON.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerON.ViewComponents
{
    public class ProductList : ViewComponent
    {
        private StoreContext _context;
        public ProductList(StoreContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(string genrename)
        {
            var genres = _context.Genres.Include("Items").Where(g => g.Name.ToUpper() == genrename.ToUpper()).Single();
            var albums = genres.Items.ToList();
            return View(albums);
        }
    }
}
