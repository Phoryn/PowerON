using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PowerON.DAL;
using PowerON.Infrastructure;
using PowerON.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerON.ViewComponents
{
    
    public class GenresMenu : ViewComponent
    {
        private StoreContext _context;
        private IMemoryCache _cache;
        public GenresMenu(StoreContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }



        public IViewComponentResult Invoke()
        {
            var genres = new List<Genre>();
            if(! _cache.TryGetValue(Const.genrMenuCacheKey, out genres))
            {
                if(genres == null)
                {
                    genres = GetGenres();
                }
                _cache.Set(Const.genrMenuCacheKey, genres, TimeSpan.FromMinutes(30));
            }
            return View(genres);
        }



        private List<Genre> GetGenres()
        {
            
            return _context.Genres.ToList();
        }


    }
}
