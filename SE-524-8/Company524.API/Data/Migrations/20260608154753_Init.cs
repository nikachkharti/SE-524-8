using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Company524.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "date", nullable: false),
                    OrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductName = table.Column<string>(type: "varchar(100)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PricePerUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName" },
                values: new object[,]
                {
                    { new Guid("11111111-0000-0000-0000-000000000001"), "Electronics" },
                    { new Guid("11111111-0000-0000-0000-000000000002"), "Books" },
                    { new Guid("11111111-0000-0000-0000-000000000003"), "Clothing" },
                    { new Guid("11111111-0000-0000-0000-000000000004"), "Home & Garden" },
                    { new Guid("11111111-0000-0000-0000-000000000005"), "Sports" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "City", "CustomerName", "Email", "LastLoginDate", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("44444444-0000-0000-0000-000000000001"), "New York", "John Smith", "john.smith@email.com", new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Local), "+1-555-0101" },
                    { new Guid("44444444-0000-0000-0000-000000000002"), "Los Angeles", "Sarah Johnson", "sarah.johnson@email.com", new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Local), "+1-555-0102" },
                    { new Guid("44444444-0000-0000-0000-000000000003"), "Chicago", "Michael Brown", "michael.brown@email.com", new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Local), "+1-555-0103" },
                    { new Guid("44444444-0000-0000-0000-000000000004"), "Houston", "Emily Davis", "emily.davis@email.com", new DateTime(2026, 5, 29, 0, 0, 0, 0, DateTimeKind.Local), "+1-555-0104" },
                    { new Guid("44444444-0000-0000-0000-000000000005"), "Phoenix", "David Wilson", "david.wilson@email.com", new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "+1-555-0105" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "SupplierName" },
                values: new object[,]
                {
                    { new Guid("22222222-0000-0000-0000-000000000001"), "TechCorp Electronics" },
                    { new Guid("22222222-0000-0000-0000-000000000002"), "Global Publishing Inc." },
                    { new Guid("22222222-0000-0000-0000-000000000003"), "Fashion World Ltd." },
                    { new Guid("22222222-0000-0000-0000-000000000004"), "Home Essentials Co." },
                    { new Guid("22222222-0000-0000-0000-000000000005"), "Sports Gear Supply" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerId", "Discount", "OrderAmount", "OrderDate", "Status" },
                values: new object[,]
                {
                    { new Guid("55555555-0000-0000-0000-000000000001"), new Guid("44444444-0000-0000-0000-000000000001"), 10m, 199.97m, new DateTime(2026, 6, 4, 0, 0, 0, 0, DateTimeKind.Local), "Delivered" },
                    { new Guid("55555555-0000-0000-0000-000000000002"), new Guid("44444444-0000-0000-0000-000000000002"), 15m, 369.96m, new DateTime(2026, 6, 7, 0, 0, 0, 0, DateTimeKind.Local), "Processing" },
                    { new Guid("55555555-0000-0000-0000-000000000003"), new Guid("44444444-0000-0000-0000-000000000003"), 5m, 249.97m, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Local), "Pending" },
                    { new Guid("55555555-0000-0000-0000-000000000004"), new Guid("44444444-0000-0000-0000-000000000001"), 0m, 129.98m, new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Local), "Delivered" },
                    { new Guid("55555555-0000-0000-0000-000000000005"), new Guid("44444444-0000-0000-0000-000000000004"), 20m, 389.97m, new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Local), "Shipped" },
                    { new Guid("55555555-0000-0000-0000-000000000006"), new Guid("44444444-0000-0000-0000-000000000005"), 8m, 149.98m, new DateTime(2026, 6, 2, 0, 0, 0, 0, DateTimeKind.Local), "Delivered" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Price", "ProductName", "Quantity", "SupplierId" },
                values: new object[,]
                {
                    { new Guid("33333333-0000-0000-0000-000000000001"), new Guid("11111111-0000-0000-0000-000000000001"), 79.99m, "Wireless Headphones", 50, new Guid("22222222-0000-0000-0000-000000000001") },
                    { new Guid("33333333-0000-0000-0000-000000000002"), new Guid("11111111-0000-0000-0000-000000000001"), 12.99m, "USB-C Cable", 200, new Guid("22222222-0000-0000-0000-000000000001") },
                    { new Guid("33333333-0000-0000-0000-000000000003"), new Guid("11111111-0000-0000-0000-000000000001"), 49.99m, "Portable Charger", 75, new Guid("22222222-0000-0000-0000-000000000001") },
                    { new Guid("33333333-0000-0000-0000-000000000004"), new Guid("11111111-0000-0000-0000-000000000002"), 45.99m, "C# Programming Guide", 30, new Guid("22222222-0000-0000-0000-000000000002") },
                    { new Guid("33333333-0000-0000-0000-000000000005"), new Guid("11111111-0000-0000-0000-000000000002"), 55.99m, "Entity Framework Core in Action", 25, new Guid("22222222-0000-0000-0000-000000000002") },
                    { new Guid("33333333-0000-0000-0000-000000000006"), new Guid("11111111-0000-0000-0000-000000000002"), 39.99m, "Clean Code", 40, new Guid("22222222-0000-0000-0000-000000000002") },
                    { new Guid("33333333-0000-0000-0000-000000000007"), new Guid("11111111-0000-0000-0000-000000000003"), 19.99m, "Cotton T-Shirt", 100, new Guid("22222222-0000-0000-0000-000000000003") },
                    { new Guid("33333333-0000-0000-0000-000000000008"), new Guid("11111111-0000-0000-0000-000000000003"), 59.99m, "Denim Jeans", 60, new Guid("22222222-0000-0000-0000-000000000003") },
                    { new Guid("33333333-0000-0000-0000-000000000009"), new Guid("11111111-0000-0000-0000-000000000003"), 129.99m, "Winter Jacket", 35, new Guid("22222222-0000-0000-0000-000000000003") },
                    { new Guid("33333333-0000-0000-0000-000000000010"), new Guid("11111111-0000-0000-0000-000000000004"), 34.99m, "LED Desk Lamp", 45, new Guid("22222222-0000-0000-0000-000000000004") },
                    { new Guid("33333333-0000-0000-0000-000000000011"), new Guid("11111111-0000-0000-0000-000000000004"), 24.99m, "Plant Pot Set", 80, new Guid("22222222-0000-0000-0000-000000000004") },
                    { new Guid("33333333-0000-0000-0000-000000000012"), new Guid("11111111-0000-0000-0000-000000000004"), 29.99m, "Wall Clock", 55, new Guid("22222222-0000-0000-0000-000000000004") },
                    { new Guid("33333333-0000-0000-0000-000000000013"), new Guid("11111111-0000-0000-0000-000000000005"), 25.99m, "Yoga Mat", 70, new Guid("22222222-0000-0000-0000-000000000005") },
                    { new Guid("33333333-0000-0000-0000-000000000014"), new Guid("11111111-0000-0000-0000-000000000005"), 89.99m, "Dumbbell Set", 40, new Guid("22222222-0000-0000-0000-000000000005") },
                    { new Guid("33333333-0000-0000-0000-000000000015"), new Guid("11111111-0000-0000-0000-000000000005"), 99.99m, "Running Shoes", 50, new Guid("22222222-0000-0000-0000-000000000005") }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "OrderId", "PricePerUnit", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { new Guid("66666666-0000-0000-0000-000000000001"), new Guid("55555555-0000-0000-0000-000000000001"), 79.99m, new Guid("33333333-0000-0000-0000-000000000001"), 2 },
                    { new Guid("66666666-0000-0000-0000-000000000002"), new Guid("55555555-0000-0000-0000-000000000001"), 12.99m, new Guid("33333333-0000-0000-0000-000000000002"), 5 },
                    { new Guid("66666666-0000-0000-0000-000000000003"), new Guid("55555555-0000-0000-0000-000000000002"), 45.99m, new Guid("33333333-0000-0000-0000-000000000004"), 3 },
                    { new Guid("66666666-0000-0000-0000-000000000004"), new Guid("55555555-0000-0000-0000-000000000002"), 55.99m, new Guid("33333333-0000-0000-0000-000000000005"), 2 },
                    { new Guid("66666666-0000-0000-0000-000000000005"), new Guid("55555555-0000-0000-0000-000000000003"), 19.99m, new Guid("33333333-0000-0000-0000-000000000007"), 4 },
                    { new Guid("66666666-0000-0000-0000-000000000006"), new Guid("55555555-0000-0000-0000-000000000003"), 59.99m, new Guid("33333333-0000-0000-0000-000000000008"), 2 },
                    { new Guid("66666666-0000-0000-0000-000000000007"), new Guid("55555555-0000-0000-0000-000000000004"), 34.99m, new Guid("33333333-0000-0000-0000-000000000010"), 3 },
                    { new Guid("66666666-0000-0000-0000-000000000008"), new Guid("55555555-0000-0000-0000-000000000004"), 24.99m, new Guid("33333333-0000-0000-0000-000000000011"), 1 },
                    { new Guid("66666666-0000-0000-0000-000000000009"), new Guid("55555555-0000-0000-0000-000000000005"), 25.99m, new Guid("33333333-0000-0000-0000-000000000013"), 5 },
                    { new Guid("66666666-0000-0000-0000-000000000010"), new Guid("55555555-0000-0000-0000-000000000005"), 89.99m, new Guid("33333333-0000-0000-0000-000000000014"), 2 },
                    { new Guid("66666666-0000-0000-0000-000000000011"), new Guid("55555555-0000-0000-0000-000000000006"), 49.99m, new Guid("33333333-0000-0000-0000-000000000003"), 2 },
                    { new Guid("66666666-0000-0000-0000-000000000012"), new Guid("55555555-0000-0000-0000-000000000006"), 39.99m, new Guid("33333333-0000-0000-0000-000000000006"), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
