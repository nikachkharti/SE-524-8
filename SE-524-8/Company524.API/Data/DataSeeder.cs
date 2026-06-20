using Company524.API.Entities;
using Company524.API.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company524.API.Data
{
    public static class DataSeeder
    {
        public static void MakeRefreshTokenColumnUnique(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>(entity => entity
                .HasIndex(rt => rt.Token)
                .IsUnique()
            );
        }

        public static void NormalizeIdentityTableNames(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>(entity => entity.ToTable(name: "Users"));
            modelBuilder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));
            modelBuilder.Entity<IdentityUserRole<string>>(entity => entity.ToTable(name: "UserRoles"));
            modelBuilder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable(name: "UserClaims"));
            modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable(name: "UserLogins"));
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable(name: "RoleClaims"));
            modelBuilder.Entity<IdentityUserToken<string>>(entity => entity.ToTable(name: "UserTokens"));
        }

        public static void SeedData(this ModelBuilder modelBuilder)
        {
            SeedCategories(modelBuilder);
            SeedSuppliers(modelBuilder);
            SeedProducts(modelBuilder);
            SeedCustomers(modelBuilder);
            SeedOrders(modelBuilder);
            SeedOrderItems(modelBuilder);
        }

        private static class SeedIds
        {
            // Categories
            public static readonly Guid Electronics = new("11111111-0000-0000-0000-000000000001");
            public static readonly Guid Books = new("11111111-0000-0000-0000-000000000002");
            public static readonly Guid Clothing = new("11111111-0000-0000-0000-000000000003");
            public static readonly Guid HomeGarden = new("11111111-0000-0000-0000-000000000004");
            public static readonly Guid Sports = new("11111111-0000-0000-0000-000000000005");

            // Suppliers
            public static readonly Guid TechCorp = new("22222222-0000-0000-0000-000000000001");
            public static readonly Guid GlobalPub = new("22222222-0000-0000-0000-000000000002");
            public static readonly Guid FashionWorld = new("22222222-0000-0000-0000-000000000003");
            public static readonly Guid HomeEssent = new("22222222-0000-0000-0000-000000000004");
            public static readonly Guid SportsGear = new("22222222-0000-0000-0000-000000000005");

            // Products
            public static readonly Guid WirelessHeadphones = new("33333333-0000-0000-0000-000000000001");
            public static readonly Guid UsbCCable = new("33333333-0000-0000-0000-000000000002");
            public static readonly Guid PortableCharger = new("33333333-0000-0000-0000-000000000003");
            public static readonly Guid CSharpGuide = new("33333333-0000-0000-0000-000000000004");
            public static readonly Guid EfCoreBook = new("33333333-0000-0000-0000-000000000005");
            public static readonly Guid CleanCode = new("33333333-0000-0000-0000-000000000006");
            public static readonly Guid CottonTShirt = new("33333333-0000-0000-0000-000000000007");
            public static readonly Guid DenimJeans = new("33333333-0000-0000-0000-000000000008");
            public static readonly Guid WinterJacket = new("33333333-0000-0000-0000-000000000009");
            public static readonly Guid LedDeskLamp = new("33333333-0000-0000-0000-000000000010");
            public static readonly Guid PlantPotSet = new("33333333-0000-0000-0000-000000000011");
            public static readonly Guid WallClock = new("33333333-0000-0000-0000-000000000012");
            public static readonly Guid YogaMat = new("33333333-0000-0000-0000-000000000013");
            public static readonly Guid DumbbellSet = new("33333333-0000-0000-0000-000000000014");
            public static readonly Guid RunningShoes = new("33333333-0000-0000-0000-000000000015");

            // Customers
            public static readonly Guid JohnSmith = new("44444444-0000-0000-0000-000000000001");
            public static readonly Guid SarahJohnson = new("44444444-0000-0000-0000-000000000002");
            public static readonly Guid MichaelBrown = new("44444444-0000-0000-0000-000000000003");
            public static readonly Guid EmilyDavis = new("44444444-0000-0000-0000-000000000004");
            public static readonly Guid DavidWilson = new("44444444-0000-0000-0000-000000000005");

            // Orders
            public static readonly Guid Order1 = new("55555555-0000-0000-0000-000000000001");
            public static readonly Guid Order2 = new("55555555-0000-0000-0000-000000000002");
            public static readonly Guid Order3 = new("55555555-0000-0000-0000-000000000003");
            public static readonly Guid Order4 = new("55555555-0000-0000-0000-000000000004");
            public static readonly Guid Order5 = new("55555555-0000-0000-0000-000000000005");
            public static readonly Guid Order6 = new("55555555-0000-0000-0000-000000000006");

            // OrderItems
            public static readonly Guid OrderItem1 = new("66666666-0000-0000-0000-000000000001");
            public static readonly Guid OrderItem2 = new("66666666-0000-0000-0000-000000000002");
            public static readonly Guid OrderItem3 = new("66666666-0000-0000-0000-000000000003");
            public static readonly Guid OrderItem4 = new("66666666-0000-0000-0000-000000000004");
            public static readonly Guid OrderItem5 = new("66666666-0000-0000-0000-000000000005");
            public static readonly Guid OrderItem6 = new("66666666-0000-0000-0000-000000000006");
            public static readonly Guid OrderItem7 = new("66666666-0000-0000-0000-000000000007");
            public static readonly Guid OrderItem8 = new("66666666-0000-0000-0000-000000000008");
            public static readonly Guid OrderItem9 = new("66666666-0000-0000-0000-000000000009");
            public static readonly Guid OrderItem10 = new("66666666-0000-0000-0000-000000000010");
            public static readonly Guid OrderItem11 = new("66666666-0000-0000-0000-000000000011");
            public static readonly Guid OrderItem12 = new("66666666-0000-0000-0000-000000000012");
        }

        private static void SeedCategories(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Category>().HasData(
                new Entities.Category { Id = SeedIds.Electronics, CategoryName = "Electronics" },
                new Entities.Category { Id = SeedIds.Books, CategoryName = "Books" },
                new Entities.Category { Id = SeedIds.Clothing, CategoryName = "Clothing" },
                new Entities.Category { Id = SeedIds.HomeGarden, CategoryName = "Home & Garden" },
                new Entities.Category { Id = SeedIds.Sports, CategoryName = "Sports" }
            );
        }

        private static void SeedSuppliers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Supplier>().HasData(
                new Entities.Supplier { Id = SeedIds.TechCorp, SupplierName = "TechCorp Electronics" },
                new Entities.Supplier { Id = SeedIds.GlobalPub, SupplierName = "Global Publishing Inc." },
                new Entities.Supplier { Id = SeedIds.FashionWorld, SupplierName = "Fashion World Ltd." },
                new Entities.Supplier { Id = SeedIds.HomeEssent, SupplierName = "Home Essentials Co." },
                new Entities.Supplier { Id = SeedIds.SportsGear, SupplierName = "Sports Gear Supply" }
            );
        }

        private static void SeedProducts(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Product>().HasData(
                // Electronics
                new Entities.Product { Id = SeedIds.WirelessHeadphones, ProductName = "Wireless Headphones", Price = 79.99m, Quantity = 50, CategoryId = SeedIds.Electronics, SupplierId = SeedIds.TechCorp },
                new Entities.Product { Id = SeedIds.UsbCCable, ProductName = "USB-C Cable", Price = 12.99m, Quantity = 200, CategoryId = SeedIds.Electronics, SupplierId = SeedIds.TechCorp },
                new Entities.Product { Id = SeedIds.PortableCharger, ProductName = "Portable Charger", Price = 49.99m, Quantity = 75, CategoryId = SeedIds.Electronics, SupplierId = SeedIds.TechCorp },

                // Books
                new Entities.Product { Id = SeedIds.CSharpGuide, ProductName = "C# Programming Guide", Price = 45.99m, Quantity = 30, CategoryId = SeedIds.Books, SupplierId = SeedIds.GlobalPub },
                new Entities.Product { Id = SeedIds.EfCoreBook, ProductName = "Entity Framework Core in Action", Price = 55.99m, Quantity = 25, CategoryId = SeedIds.Books, SupplierId = SeedIds.GlobalPub },
                new Entities.Product { Id = SeedIds.CleanCode, ProductName = "Clean Code", Price = 39.99m, Quantity = 40, CategoryId = SeedIds.Books, SupplierId = SeedIds.GlobalPub },

                // Clothing
                new Entities.Product { Id = SeedIds.CottonTShirt, ProductName = "Cotton T-Shirt", Price = 19.99m, Quantity = 100, CategoryId = SeedIds.Clothing, SupplierId = SeedIds.FashionWorld },
                new Entities.Product { Id = SeedIds.DenimJeans, ProductName = "Denim Jeans", Price = 59.99m, Quantity = 60, CategoryId = SeedIds.Clothing, SupplierId = SeedIds.FashionWorld },
                new Entities.Product { Id = SeedIds.WinterJacket, ProductName = "Winter Jacket", Price = 129.99m, Quantity = 35, CategoryId = SeedIds.Clothing, SupplierId = SeedIds.FashionWorld },

                // Home & Garden
                new Entities.Product { Id = SeedIds.LedDeskLamp, ProductName = "LED Desk Lamp", Price = 34.99m, Quantity = 45, CategoryId = SeedIds.HomeGarden, SupplierId = SeedIds.HomeEssent },
                new Entities.Product { Id = SeedIds.PlantPotSet, ProductName = "Plant Pot Set", Price = 24.99m, Quantity = 80, CategoryId = SeedIds.HomeGarden, SupplierId = SeedIds.HomeEssent },
                new Entities.Product { Id = SeedIds.WallClock, ProductName = "Wall Clock", Price = 29.99m, Quantity = 55, CategoryId = SeedIds.HomeGarden, SupplierId = SeedIds.HomeEssent },

                // Sports
                new Entities.Product { Id = SeedIds.YogaMat, ProductName = "Yoga Mat", Price = 25.99m, Quantity = 70, CategoryId = SeedIds.Sports, SupplierId = SeedIds.SportsGear },
                new Entities.Product { Id = SeedIds.DumbbellSet, ProductName = "Dumbbell Set", Price = 89.99m, Quantity = 40, CategoryId = SeedIds.Sports, SupplierId = SeedIds.SportsGear },
                new Entities.Product { Id = SeedIds.RunningShoes, ProductName = "Running Shoes", Price = 99.99m, Quantity = 50, CategoryId = SeedIds.Sports, SupplierId = SeedIds.SportsGear }
            );
        }

        private static void SeedCustomers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Customer>().HasData(
                new Entities.Customer { Id = SeedIds.JohnSmith, CustomerName = "John Smith", City = "New York", Email = "john.smith@email.com", PhoneNumber = "+1-555-0101", LastLoginDate = DateTime.Now.AddDays(-5).Date },
                new Entities.Customer { Id = SeedIds.SarahJohnson, CustomerName = "Sarah Johnson", City = "Los Angeles", Email = "sarah.johnson@email.com", PhoneNumber = "+1-555-0102", LastLoginDate = DateTime.Now.AddDays(-2).Date },
                new Entities.Customer { Id = SeedIds.MichaelBrown, CustomerName = "Michael Brown", City = "Chicago", Email = "michael.brown@email.com", PhoneNumber = "+1-555-0103", LastLoginDate = DateTime.Now.Date },
                new Entities.Customer { Id = SeedIds.EmilyDavis, CustomerName = "Emily Davis", City = "Houston", Email = "emily.davis@email.com", PhoneNumber = "+1-555-0104", LastLoginDate = DateTime.Now.AddDays(-10).Date },
                new Entities.Customer { Id = SeedIds.DavidWilson, CustomerName = "David Wilson", City = "Phoenix", Email = "david.wilson@email.com", PhoneNumber = "+1-555-0105", LastLoginDate = DateTime.Now.AddDays(-3).Date }
            );
        }

        private static void SeedOrders(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Order>().HasData(
                new Entities.Order { Id = SeedIds.Order1, CustomerId = SeedIds.JohnSmith, OrderDate = DateTime.Now.AddDays(-4).Date, OrderAmount = 199.97m, Status = "Delivered", Discount = 10m },
                new Entities.Order { Id = SeedIds.Order2, CustomerId = SeedIds.SarahJohnson, OrderDate = DateTime.Now.AddDays(-1).Date, OrderAmount = 369.96m, Status = "Processing", Discount = 15m },
                new Entities.Order { Id = SeedIds.Order3, CustomerId = SeedIds.MichaelBrown, OrderDate = DateTime.Now.Date, OrderAmount = 249.97m, Status = "Pending", Discount = 5m },
                new Entities.Order { Id = SeedIds.Order4, CustomerId = SeedIds.JohnSmith, OrderDate = DateTime.Now.AddDays(-3).Date, OrderAmount = 129.98m, Status = "Delivered", Discount = 0m },
                new Entities.Order { Id = SeedIds.Order5, CustomerId = SeedIds.EmilyDavis, OrderDate = DateTime.Now.AddDays(-2).Date, OrderAmount = 389.97m, Status = "Shipped", Discount = 20m },
                new Entities.Order { Id = SeedIds.Order6, CustomerId = SeedIds.DavidWilson, OrderDate = DateTime.Now.AddDays(-6).Date, OrderAmount = 149.98m, Status = "Delivered", Discount = 8m }
            );
        }

        private static void SeedOrderItems(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.OrderItem>().HasData(
                // Order 1 - John Smith
                new Entities.OrderItem { Id = SeedIds.OrderItem1, OrderId = SeedIds.Order1, ProductId = SeedIds.WirelessHeadphones, Quantity = 2, PricePerUnit = 79.99m },
                new Entities.OrderItem { Id = SeedIds.OrderItem2, OrderId = SeedIds.Order1, ProductId = SeedIds.UsbCCable, Quantity = 5, PricePerUnit = 12.99m },

                // Order 2 - Sarah Johnson
                new Entities.OrderItem { Id = SeedIds.OrderItem3, OrderId = SeedIds.Order2, ProductId = SeedIds.CSharpGuide, Quantity = 3, PricePerUnit = 45.99m },
                new Entities.OrderItem { Id = SeedIds.OrderItem4, OrderId = SeedIds.Order2, ProductId = SeedIds.EfCoreBook, Quantity = 2, PricePerUnit = 55.99m },

                // Order 3 - Michael Brown
                new Entities.OrderItem { Id = SeedIds.OrderItem5, OrderId = SeedIds.Order3, ProductId = SeedIds.CottonTShirt, Quantity = 4, PricePerUnit = 19.99m },
                new Entities.OrderItem { Id = SeedIds.OrderItem6, OrderId = SeedIds.Order3, ProductId = SeedIds.DenimJeans, Quantity = 2, PricePerUnit = 59.99m },

                // Order 4 - John Smith
                new Entities.OrderItem { Id = SeedIds.OrderItem7, OrderId = SeedIds.Order4, ProductId = SeedIds.LedDeskLamp, Quantity = 3, PricePerUnit = 34.99m },
                new Entities.OrderItem { Id = SeedIds.OrderItem8, OrderId = SeedIds.Order4, ProductId = SeedIds.PlantPotSet, Quantity = 1, PricePerUnit = 24.99m },

                // Order 5 - Emily Davis
                new Entities.OrderItem { Id = SeedIds.OrderItem9, OrderId = SeedIds.Order5, ProductId = SeedIds.YogaMat, Quantity = 5, PricePerUnit = 25.99m },
                new Entities.OrderItem { Id = SeedIds.OrderItem10, OrderId = SeedIds.Order5, ProductId = SeedIds.DumbbellSet, Quantity = 2, PricePerUnit = 89.99m },

                // Order 6 - David Wilson
                new Entities.OrderItem { Id = SeedIds.OrderItem11, OrderId = SeedIds.Order6, ProductId = SeedIds.PortableCharger, Quantity = 2, PricePerUnit = 49.99m },
                new Entities.OrderItem { Id = SeedIds.OrderItem12, OrderId = SeedIds.Order6, ProductId = SeedIds.CleanCode, Quantity = 1, PricePerUnit = 39.99m }
            );
        }
    }
}
