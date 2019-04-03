using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PowerON.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IconFilename = table.Column<string>(nullable: true),
                    TestColumnDuda = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 150, nullable: true),
                    LastNAme = table.Column<string>(maxLength: 150, nullable: true),
                    Address = table.Column<string>(nullable: true),
                    CodeAndCity = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    OrderState = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(9,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GenreId = table.Column<int>(nullable: false),
                    ItemTitle = table.Column<string>(nullable: true),
                    ItemName = table.Column<string>(nullable: true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    ImageFileName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    IsBestseller = table.Column<bool>(nullable: false),
                    IsHidden = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Items_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>(nullable: false),
                    AlbumId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    ItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "GenreId", "Description", "IconFilename", "Name", "TestColumnDuda" },
                values: new object[,]
                {
                    { 1, null, "komputery.png", "Komputery", null },
                    { 2, null, "monitory.png", "Monitory", null },
                    { 3, null, "telefony.png", "Telefony", null },
                    { 4, null, "myszki.png", "Myszki", null },
                    { 5, null, "klawiatury.png", "Klawiatury", null }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemId", "DateAdded", "Description", "GenreId", "ImageFileName", "IsBestseller", "IsHidden", "ItemName", "ItemTitle", "Price" },
                values: new object[,]
                {
                    { 1, new DateTime(2019, 4, 3, 0, 0, 0, 0, DateTimeKind.Local), "Najlepszy bo Description", 1, "1.png", true, false, "ItemNAme coś tam", "Komputer Sonic Item Title", 99.0m },
                    { 2, new DateTime(2019, 4, 3, 0, 0, 0, 0, DateTimeKind.Local), "Najlepszy bo Description1", 1, "2.png", true, false, "ItemNAme coś tam1", "Komputer Sonic Item Title1", 44.0m },
                    { 3, new DateTime(2019, 4, 3, 0, 0, 0, 0, DateTimeKind.Local), "Najlepszy bo Description2", 2, "3.png", false, false, "ItemNAme coś tam2", "Komputer Sonic Item Title2", 66m },
                    { 4, new DateTime(2019, 4, 3, 0, 0, 0, 0, DateTimeKind.Local), "Najlepszy bo Description3", 2, "4.png", false, false, "ItemNAme coś tam3", "Komputer Sonic Item Title3", 77m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_GenreId",
                table: "Items",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ItemId",
                table: "OrderItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Genres");
        }
    }
}
