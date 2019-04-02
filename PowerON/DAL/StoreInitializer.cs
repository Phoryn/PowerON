using Microsoft.EntityFrameworkCore;
using PowerON.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace PowerON.DAL
{
    public static class StoreInitializer
    {
        public static void Initialize(StoreContext context)
        {
            context.Database.EnsureCreated();

            if (context.Items.Any())
            {
                return;   // DB has been seeded
            }
        
            var genres = new List<Genre>
            {
                new Genre(){Name="Komputery",IconFilename="komputery.png"},
                new Genre(){Name="Monitory",IconFilename="monitory.png"},
                new Genre(){Name="Telefony",IconFilename="telefony.png"},
                new Genre(){Name="Myszki",IconFilename="myszki.png"},
                new Genre(){Name="Klawiatury",IconFilename="klawiatury.png"}
            };

            genres.ForEach(g => context.Genres.Add(g));
            context.SaveChanges();

            var items = new List<Item>
            {
                new Item(){ItemTitle="Komputer Sonic Item Title", Price = 99, ItemName = "ItemNAme coś tam", Description = "Najlepszy bo Description", DateAdded = DateTime.Now.Date, ImageFileName = "1.png", IsBestseller = true, GenreId = 1 },
                new Item(){ItemTitle="Komputer Sonic Item Title1", Price = 99, ItemName = "ItemNAme coś tam1", Description = "Najlepszy bo Description1", DateAdded = DateTime.Now.Date, ImageFileName = "2.png", IsBestseller = true, GenreId = 1 },
                new Item(){ItemTitle="Komputer Sonic Item Title2", Price = 99, ItemName = "ItemNAme coś tam2", Description = "Najlepszy bo Description2", DateAdded = DateTime.Now.Date, ImageFileName = "3.png", IsBestseller = false, GenreId = 2 },
                new Item(){ItemTitle="Komputer Sonic Item Title3", Price = 99, ItemName = "ItemNAme coś tam3", Description = "Najlepszy bo Description3", DateAdded = DateTime.Now.Date, ImageFileName = "4.png", IsBestseller = false,GenreId = 2 }
            };

            items.ForEach(i => context.Items.Add(i));
            context.SaveChanges();
        }
    }
}
