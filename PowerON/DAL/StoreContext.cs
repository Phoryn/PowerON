
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PowerON.Models;
using System;

namespace PowerON.DAL
{
    public class StoreContext : IdentityDbContext<ApplicationUser>
    {
        public StoreContext(DbContextOptions<StoreContext> options) 
            : base(options)
        {

        }
        public DbSet<Item> Items{get;set;}

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        //public DbSet<UserData> UserData { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().HasData(
                new { GenreId = 1, Name = "Komputery", IconFilename = "komputery.png" },
                new { GenreId = 2, Name = "Monitory", IconFilename = "komputery.png" },
                new { GenreId = 3, Name = "Telefony", IconFilename = "komputery.png" },
                new { GenreId = 4, Name = "Myszki", IconFilename = "komputery.png" },
                new { GenreId = 5, Name = "Klawiatury", IconFilename = "komputery.png" }
                );

            modelBuilder.Entity<Item>().HasData(
                new { ItemId = 1, ItemTitle = "Komputer Sonic Item Title", Price = 99.0m, ItemName = "ItemNAme coś tam", Description = "Najlepszy bo Description", DateAdded = DateTime.Now.Date, ImageFileName = "1.png", IsBestseller = true, GenreId = 1, IsHidden = false },
                new { ItemId = 2, ItemTitle = "Komputer Sonic Item Title1", Price = 44.0m, ItemName = "ItemNAme coś tam1", Description = "Najlepszy bo Description1", DateAdded = DateTime.Now.Date, ImageFileName = "1.png", IsBestseller = true, GenreId = 1, IsHidden = false },
                new { ItemId = 3, ItemTitle = "Komputer Sonic Item Title2", Price = 66m, ItemName = "ItemNAme coś tam2", Description = "Najlepszy bo Description2", DateAdded = DateTime.Now.Date, ImageFileName = "1.png", IsBestseller = false, GenreId = 2, IsHidden = false },
                new { ItemId = 4, ItemTitle = "Komputer Sonic Item Title3", Price = 77m, ItemName = "ItemNAme coś tam3", Description = "Najlepszy bo Description3", DateAdded = DateTime.Now.Date, ImageFileName = "1.png", IsBestseller = false, GenreId = 2, IsHidden = false }
                );

            //modelBuilder.Entity<ApplicationUser>()
            //   .HasOne(a => a.UserData)
            //   .WithOne(b => b.ApplicationUser)
            //   .HasForeignKey<UserData>(b => b.ReferenceToUser);


        }
    }
}
