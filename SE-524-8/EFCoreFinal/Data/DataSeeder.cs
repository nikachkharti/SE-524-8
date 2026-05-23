using Microsoft.EntityFrameworkCore;

namespace EFCoreFinal.Data
{
    public static class DataSeeder
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            SeedCategories(modelBuilder);
            SeedSuppliers(modelBuilder);
            SeedProducts(modelBuilder);
            SeedCustomers(modelBuilder);
            SeedOrders(modelBuilder);
            SeedOrderItems(modelBuilder);
            SeedInventories(modelBuilder);
        }

        private static void SeedCategories(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Category>().HasData(
                new Entities.Category { Id = 1, CategoryName = "Electronics" },
                new Entities.Category { Id = 2, CategoryName = "Books" },
                new Entities.Category { Id = 3, CategoryName = "Clothing" },
                new Entities.Category { Id = 4, CategoryName = "Home & Garden" },
                new Entities.Category { Id = 5, CategoryName = "Sports" }
            );
        }

        private static void SeedSuppliers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Supplier>().HasData(
                new Entities.Supplier { Id = 1, SupplierName = "TechCorp Electronics" },
                new Entities.Supplier { Id = 2, SupplierName = "Global Publishing Inc." },
                new Entities.Supplier { Id = 3, SupplierName = "Fashion World Ltd." },
                new Entities.Supplier { Id = 4, SupplierName = "Home Essentials Co." },
                new Entities.Supplier { Id = 5, SupplierName = "Sports Gear Supply" }
            );
        }

        private static void SeedProducts(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Product>().HasData(
                // Electronics
                new Entities.Product { Id = 1, ProductName = "Wireless Headphones", Price = 79.99m, Quantity = 50, CategoryId = 1, SupplierId = 1 },
                new Entities.Product { Id = 2, ProductName = "USB-C Cable", Price = 12.99m, Quantity = 200, CategoryId = 1, SupplierId = 1 },
                new Entities.Product { Id = 3, ProductName = "Portable Charger", Price = 49.99m, Quantity = 75, CategoryId = 1, SupplierId = 1 },

                // Books
                new Entities.Product { Id = 4, ProductName = "C# Programming Guide", Price = 45.99m, Quantity = 30, CategoryId = 2, SupplierId = 2 },
                new Entities.Product { Id = 5, ProductName = "Entity Framework Core in Action", Price = 55.99m, Quantity = 25, CategoryId = 2, SupplierId = 2 },
                new Entities.Product { Id = 6, ProductName = "Clean Code", Price = 39.99m, Quantity = 40, CategoryId = 2, SupplierId = 2 },

                // Clothing
                new Entities.Product { Id = 7, ProductName = "Cotton T-Shirt", Price = 19.99m, Quantity = 100, CategoryId = 3, SupplierId = 3 },
                new Entities.Product { Id = 8, ProductName = "Denim Jeans", Price = 59.99m, Quantity = 60, CategoryId = 3, SupplierId = 3 },
                new Entities.Product { Id = 9, ProductName = "Winter Jacket", Price = 129.99m, Quantity = 35, CategoryId = 3, SupplierId = 3 },

                // Home & Garden
                new Entities.Product { Id = 10, ProductName = "LED Desk Lamp", Price = 34.99m, Quantity = 45, CategoryId = 4, SupplierId = 4 },
                new Entities.Product { Id = 11, ProductName = "Plant Pot Set", Price = 24.99m, Quantity = 80, CategoryId = 4, SupplierId = 4 },
                new Entities.Product { Id = 12, ProductName = "Wall Clock", Price = 29.99m, Quantity = 55, CategoryId = 4, SupplierId = 4 },

                // sports
                new Entities.Product { Id = 13, ProductName = "Yoga Mat", Price = 25.99m, Quantity = 70, CategoryId = 5, SupplierId = 5 },
                new Entities.Product { Id = 14, ProductName = "Dumbbell Set", Price = 89.99m, Quantity = 40, CategoryId = 5, SupplierId = 5 },
                new Entities.Product { Id = 15, ProductName = "Running Shoes", Price = 99.99m, Quantity = 50, CategoryId = 5, SupplierId = 5 }
            );
        }

        private static void SeedCustomers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Customer>().HasData(
                new Entities.Customer
                {
                    Id = 1,
                    CustomerName = "John Smith",
                    City = "New York",
                    Email = "john.smith@email.com",
                    PhoneNumber = "+1-555-0101",
                    LastLoginDate = DateTime.Now.AddDays(-5).Date
                },
                new Entities.Customer
                {
                    Id = 2,
                    CustomerName = "Sarah Johnson",
                    City = "Los Angeles",
                    Email = "sarah.johnson@email.com",
                    PhoneNumber = "+1-555-0102",
                    LastLoginDate = DateTime.Now.AddDays(-2).Date
                },
                new Entities.Customer
                {
                    Id = 3,
                    CustomerName = "Michael Brown",
                    City = "Chicago",
                    Email = "michael.brown@email.com",
                    PhoneNumber = "+1-555-0103",
                    LastLoginDate = DateTime.Now.Date
                },
                new Entities.Customer
                {
                    Id = 4,
                    CustomerName = "Emily Davis",
                    City = "Houston",
                    Email = "emily.davis@email.com",
                    PhoneNumber = "+1-555-0104",
                    LastLoginDate = DateTime.Now.AddDays(-10).Date
                },
                new Entities.Customer
                {
                    Id = 5,
                    CustomerName = "David Wilson",
                    City = "Phoenix",
                    Email = "david.wilson@email.com",
                    PhoneNumber = "+1-555-0105",
                    LastLoginDate = DateTime.Now.AddDays(-3).Date
                }
            );
        }

        private static void SeedOrders(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Order>().HasData(
                new Entities.Order
                {
                    Id = 1,
                    CustomerId = 1,
                    LastLoginDate = DateTime.Now.AddDays(-4).Date,
                    OrderAmount = 199.97m,
                    Status = "Delivered",
                    Discount = 10m
                },
                new Entities.Order
                {
                    Id = 2,
                    CustomerId = 2,
                    LastLoginDate = DateTime.Now.AddDays(-1).Date,
                    OrderAmount = 369.96m,
                    Status = "Processing",
                    Discount = 15m
                },
                new Entities.Order
                {
                    Id = 3,
                    CustomerId = 3,
                    LastLoginDate = DateTime.Now.Date,
                    OrderAmount = 249.97m,
                    Status = "Pending",
                    Discount = 5m
                },
                new Entities.Order
                {
                    Id = 4,
                    CustomerId = 1,
                    LastLoginDate = DateTime.Now.AddDays(-3).Date,
                    OrderAmount = 129.98m,
                    Status = "Delivered",
                    Discount = 0m
                },
                new Entities.Order
                {
                    Id = 5,
                    CustomerId = 4,
                    LastLoginDate = DateTime.Now.AddDays(-2).Date,
                    OrderAmount = 389.97m,
                    Status = "Shipped",
                    Discount = 20m
                },
                new Entities.Order
                {
                    Id = 6,
                    CustomerId = 5,
                    LastLoginDate = DateTime.Now.AddDays(-6).Date,
                    OrderAmount = 149.98m,
                    Status = "Delivered",
                    Discount = 8m
                }
            );
        }

        private static void SeedOrderItems(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.OrderItem>().HasData(
                // Order 1 - John Smith
                new Entities.OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 2, PricePerUnit = 79.99m },
                new Entities.OrderItem { Id = 2, OrderId = 1, ProductId = 2, Quantity = 5, PricePerUnit = 12.99m },

                // Order 2 - Sarah Johnson
                new Entities.OrderItem { Id = 3, OrderId = 2, ProductId = 4, Quantity = 3, PricePerUnit = 45.99m },
                new Entities.OrderItem { Id = 4, OrderId = 2, ProductId = 5, Quantity = 2, PricePerUnit = 55.99m },

                // Order 3 - Michael Brown
                new Entities.OrderItem { Id = 5, OrderId = 3, ProductId = 7, Quantity = 4, PricePerUnit = 19.99m },
                new Entities.OrderItem { Id = 6, OrderId = 3, ProductId = 8, Quantity = 2, PricePerUnit = 59.99m },

                // Order 4 - John Smith
                new Entities.OrderItem { Id = 7, OrderId = 4, ProductId = 10, Quantity = 3, PricePerUnit = 34.99m },
                new Entities.OrderItem { Id = 8, OrderId = 4, ProductId = 11, Quantity = 1, PricePerUnit = 24.99m },

                // Order 5 - Emily Davis
                new Entities.OrderItem { Id = 9, OrderId = 5, ProductId = 13, Quantity = 5, PricePerUnit = 25.99m },
                new Entities.OrderItem { Id = 10, OrderId = 5, ProductId = 14, Quantity = 2, PricePerUnit = 89.99m },

                // Order 6 - David Wilson
                new Entities.OrderItem { Id = 11, OrderId = 6, ProductId = 3, Quantity = 2, PricePerUnit = 49.99m },
                new Entities.OrderItem { Id = 12, OrderId = 6, ProductId = 6, Quantity = 1, PricePerUnit = 39.99m }
            );
        }

        private static void SeedInventories(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Inventory>().HasData(
                new Entities.Inventory { Id = 1, ProductId = 1, AvailableQuantity = 48 },
                new Entities.Inventory { Id = 2, ProductId = 2, AvailableQuantity = 195 },
                new Entities.Inventory { Id = 3, ProductId = 3, AvailableQuantity = 73 },
                new Entities.Inventory { Id = 4, ProductId = 4, AvailableQuantity = 27 },
                new Entities.Inventory { Id = 5, ProductId = 5, AvailableQuantity = 23 },
                new Entities.Inventory { Id = 6, ProductId = 6, AvailableQuantity = 39 },
                new Entities.Inventory { Id = 7, ProductId = 7, AvailableQuantity = 96 },
                new Entities.Inventory { Id = 8, ProductId = 8, AvailableQuantity = 58 },
                new Entities.Inventory { Id = 9, ProductId = 9, AvailableQuantity = 35 },
                new Entities.Inventory { Id = 10, ProductId = 10, AvailableQuantity = 42 },
                new Entities.Inventory { Id = 11, ProductId = 11, AvailableQuantity = 79 },
                new Entities.Inventory { Id = 12, ProductId = 12, AvailableQuantity = 55 },
                new Entities.Inventory { Id = 13, ProductId = 13, AvailableQuantity = 65 },
                new Entities.Inventory { Id = 14, ProductId = 14, AvailableQuantity = 38 },
                new Entities.Inventory { Id = 15, ProductId = 15, AvailableQuantity = 48 }
            );
        }
    }
}
