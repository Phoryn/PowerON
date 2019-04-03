﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PowerON.DAL;

namespace PowerON.Migrations
{
    [DbContext(typeof(StoreContext))]
    partial class StoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PowerON.Models.Genre", b =>
                {
                    b.Property<int>("GenreId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("IconFilename");

                    b.Property<string>("Name");

                    b.Property<string>("TestColumnDuda");

                    b.HasKey("GenreId");

                    b.ToTable("Genres");

                    b.HasData(
                        new { GenreId = 1, IconFilename = "komputery.png", Name = "Komputery" },
                        new { GenreId = 2, IconFilename = "monitory.png", Name = "Monitory" },
                        new { GenreId = 3, IconFilename = "telefony.png", Name = "Telefony" },
                        new { GenreId = 4, IconFilename = "myszki.png", Name = "Myszki" },
                        new { GenreId = 5, IconFilename = "klawiatury.png", Name = "Klawiatury" }
                    );
                });

            modelBuilder.Entity("PowerON.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description");

                    b.Property<int>("GenreId");

                    b.Property<string>("ImageFileName");

                    b.Property<bool>("IsBestseller");

                    b.Property<bool>("IsHidden");

                    b.Property<string>("ItemName");

                    b.Property<string>("ItemTitle");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(9,2)");

                    b.HasKey("ItemId");

                    b.HasIndex("GenreId");

                    b.ToTable("Items");

                    b.HasData(
                        new { ItemId = 1, DateAdded = new DateTime(2019, 4, 3, 0, 0, 0, 0, DateTimeKind.Local), Description = "Najlepszy bo Description", GenreId = 1, ImageFileName = "1.png", IsBestseller = true, IsHidden = false, ItemName = "ItemNAme coś tam", ItemTitle = "Komputer Sonic Item Title", Price = 99.0m },
                        new { ItemId = 2, DateAdded = new DateTime(2019, 4, 3, 0, 0, 0, 0, DateTimeKind.Local), Description = "Najlepszy bo Description1", GenreId = 1, ImageFileName = "2.png", IsBestseller = true, IsHidden = false, ItemName = "ItemNAme coś tam1", ItemTitle = "Komputer Sonic Item Title1", Price = 44.0m },
                        new { ItemId = 3, DateAdded = new DateTime(2019, 4, 3, 0, 0, 0, 0, DateTimeKind.Local), Description = "Najlepszy bo Description2", GenreId = 2, ImageFileName = "3.png", IsBestseller = false, IsHidden = false, ItemName = "ItemNAme coś tam2", ItemTitle = "Komputer Sonic Item Title2", Price = 66m },
                        new { ItemId = 4, DateAdded = new DateTime(2019, 4, 3, 0, 0, 0, 0, DateTimeKind.Local), Description = "Najlepszy bo Description3", GenreId = 2, ImageFileName = "4.png", IsBestseller = false, IsHidden = false, ItemName = "ItemNAme coś tam3", ItemTitle = "Komputer Sonic Item Title3", Price = 77m }
                    );
                });

            modelBuilder.Entity("PowerON.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("CodeAndCity")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Comment");

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName")
                        .HasMaxLength(150);

                    b.Property<string>("LastNAme")
                        .HasMaxLength(150);

                    b.Property<int>("OrderState");

                    b.Property<string>("PhoneNumber");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(9,2)");

                    b.HasKey("OrderId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("PowerON.Models.OrderItem", b =>
                {
                    b.Property<int>("OrderItemId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AlbumId");

                    b.Property<int?>("ItemId");

                    b.Property<int>("OrderId");

                    b.Property<int>("Quantity");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(9,2)");

                    b.HasKey("OrderItemId");

                    b.HasIndex("ItemId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("PowerON.Models.Item", b =>
                {
                    b.HasOne("PowerON.Models.Genre", "Genre")
                        .WithMany("Items")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PowerON.Models.OrderItem", b =>
                {
                    b.HasOne("PowerON.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId");

                    b.HasOne("PowerON.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
