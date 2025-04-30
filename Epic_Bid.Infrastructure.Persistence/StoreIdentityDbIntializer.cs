using Epic_Bid.Core.Domain.Common;
using Epic_Bid.Core.Domain.Contracts.Persistence;
using Epic_Bid.Core.Domain.Entities.Auth;
using Epic_Bid.Core.Domain.Entities.Order;
using Epic_Bid.Core.Domain.Entities.Products;
using Epic_Bid.Core.Domain.Entities.Roles;
using Epic_Bid.Infrastructure.Persistence._IdentityAndData.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Epic_Bid.Infrastructure.Persistence
{
    public class StoreIdentityDbIntializer(StoreIdentityDbContext _dbcontext, UserManager<ApplicationUser> _userManager, RoleManager<AppRole> _roleManager) : IStoreIdentityDbIntializer
    {
        public async Task InitializeAsync()
        {
            //var pendingMigrations =await _dbcontext.Database.GetPendingMigrationsAsync();

            //if (pendingMigrations.Any())

            await _dbcontext.Database.MigrateAsync();


        }

        public async Task SeedAsync()
        {
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser(){ DisplayName="Ahmed Ismail", Email="ahmedesm415@example.com", UserName="ahmedesm415@example.com", PhoneNumber="01000000020" },
                new ApplicationUser(){ DisplayName="Mohamed Gamal", Email="mohamed.gamal@example.com", UserName="mohamed.gamal@example.com", PhoneNumber="01000000001" },
                new ApplicationUser(){ DisplayName="Sara Ahmed", Email="sara.ahmed@example.com", UserName="sara.ahmed@example.com", PhoneNumber="01000000002" },
                new ApplicationUser(){ DisplayName="Youssef Hassan", Email="youssef.hassan@example.com", UserName="youssef.hassan@example.com", PhoneNumber="01000000003" },
                new ApplicationUser(){ DisplayName="Mona Adel", Email="mona.adel@example.com", UserName="mona.adel@example.com", PhoneNumber="01000000004" },
                new ApplicationUser(){ DisplayName="Omar Khaled", Email="omar.khaled@example.com", UserName="omar.khaled@example.com", PhoneNumber="01000000005" },
                new ApplicationUser(){ DisplayName="Nourhan Tarek", Email="nourhan.tarek@example.com", UserName="nourhan.tarek@example.com", PhoneNumber="01000000006" },
                new ApplicationUser(){ DisplayName="Ahmed Mostafa", Email="ahmed.mostafa@example.com", UserName="ahmed.mostafa@example.com", PhoneNumber="01000000007" },
                new ApplicationUser(){ DisplayName="Dina Mahmoud", Email="dina.mahmoud@example.com", UserName="dina.mahmoud@example.com", PhoneNumber="01000000008" },
                new ApplicationUser(){ DisplayName="Khaled Nabil", Email="khaled.nabil@example.com", UserName="khaled.nabil@example.com", PhoneNumber="01000000009" },
                new ApplicationUser(){ DisplayName="Hana Sameh", Email="hana.sameh@example.com", UserName="hana.sameh@example.com", PhoneNumber="01000000010" },
                new ApplicationUser(){ DisplayName="Tamer Yassin", Email="tamer.yassin@example.com", UserName="tamer.yassin@example.com", PhoneNumber="01000000011" },
                new ApplicationUser(){ DisplayName="Laila Sherif", Email="laila.sherif@example.com", UserName="laila.sherif@example.com", PhoneNumber="01000000012" },
                new ApplicationUser(){ DisplayName="Ibrahim Hany", Email="ibrahim.hany@example.com", UserName="ibrahim.hany@example.com", PhoneNumber="01000000013" },
                new ApplicationUser(){ DisplayName="Reem Fathy", Email="reem.fathy@example.com", UserName="reem.fathy@example.com", PhoneNumber="01000000014" },
                new ApplicationUser(){ DisplayName="Walid Saeed", Email="walid.saeed@example.com", UserName="walid.saeed@example.com", PhoneNumber="01000000015" },
                new ApplicationUser(){ DisplayName="Nadine Osama", Email="nadine.osama@example.com", UserName="nadine.osama@example.com", PhoneNumber="01000000016" },
                new ApplicationUser(){ DisplayName="Mostafa Younes", Email="mostafa.younes@example.com", UserName="mostafa.younes@example.com", PhoneNumber="01000000017" },
                new ApplicationUser(){ DisplayName="Marwa Atef", Email="marwa.atef@example.com", UserName="marwa.atef@example.com", PhoneNumber="01000000018" },
                new ApplicationUser(){ DisplayName="Aly Rashed", Email="aly.rashed@example.com", UserName="aly.rashed@example.com", PhoneNumber="01000000019" },
                new ApplicationUser(){ DisplayName="Esraa ElSayed", Email="esraa.elsayed@example.com", UserName="esraa.elsayed@example.com", PhoneNumber="01000000020" }
            };

            // Seeding the Users 
            if (!_userManager.Users.Any())
            {
                foreach (var user in users)
                {
                    await _userManager.CreateAsync(user, "Password");
                }
            }

            // Seeding the Roles 
            if (!_roleManager.Roles.Any())
            {
                var Roles = new List<AppRole>()
                {
                    new AppRole() { Name = "Admin" , NormalizedName = "Admin".ToUpper(),ConcurrencyStamp = Guid.NewGuid().ToString()},
                    new AppRole() { Name = "Bayer" ,NormalizedName = "Bayer".ToUpper(),ConcurrencyStamp = Guid.NewGuid().ToString()},
                    new AppRole() { Name = "Seller" ,NormalizedName = "Seller".ToUpper(),ConcurrencyStamp = Guid.NewGuid().ToString()}
                };
                foreach (var Role in Roles)
                {
                    await _roleManager.CreateAsync(Role);
                }
            }


            // StoreContextInitializer seeding
            if (!_dbcontext.ProductCategories.Any())
            {
                //// Reading the data form json file
                //var Data = File.ReadAllText("../Epic_Bid.Infrastructure.Persistence/_IdentityAndData/DataSeed/ProductCategories.json");
                //// Deserializing the data
                //var ProductCategories = JsonSerializer.Deserialize<List<ProductCategory>>(Data);
                //// Adding the data to the database
                var ProductCategories = new List<ProductCategory>()
                {

                    new ProductCategory() { Name = "Pillow" },
                    new ProductCategory() { Name = "Chairs" },
                    new ProductCategory() { Name = "Circular Table" },
                    new ProductCategory() { Name = "Table" },
                    new ProductCategory() { Name = "Lamps" },
                    new ProductCategory() { Name = "Furniture" },
                };

                if (ProductCategories?.Count > 0)
                {
                    foreach (var item in ProductCategories)
                    {
                        await _dbcontext.ProductCategories.AddAsync(item);
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }

            if (!_dbcontext.Products.Any())
            {
                //// Reading the data form json file
                //var Data = File.ReadAllText("../Epic_Bid.Infrastructure.Persistence/_IdentityAndData/DataSeed/Product.json");
                //// Deserializing the data
                //var Products = JsonSerializer.Deserialize<List<Product>>(Data);
                //// Adding the data to the database
                var Products = new List<Product>()
                {
                    new Product { Name = "CloudSoft Pillow", Description = "Ultra-soft memory foam pillow for a restful sleep", ImageUrl = "A1", Price = 120, OldPrice = 150, InStock = true, Color = "White", Size = "Medium", Dimensions = "20×20×5", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(1), AuctionEndTime = DateTime.UtcNow.AddHours(24), IsAuctionClosed = false },
new Product { Name = "ErgoComfort Chair", Description = "Ergonomic office chair with lumbar support", ImageUrl = "B1", Price = 95, OldPrice = null, InStock = true, Color = "Black", Size = "Large", Dimensions = "30×30×40", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Luna Circular Table", Description = "Modern circular dining table with a sleek finish", ImageUrl = "C1", Price = 220, OldPrice = 260, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "40×40×30", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(2), AuctionEndTime = DateTime.UtcNow.AddHours(30), IsAuctionClosed = false },
new Product { Name = "Oakwood Dining Table", Description = "Solid oak dining table for family gatherings", ImageUrl = "D1", Price = 180, OldPrice = null, InStock = true, Color = "Green", Size = "Large", Dimensions = "60×30×35", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Starlight Desk Lamp", Description = "Elegant LED desk lamp with adjustable brightness", ImageUrl = "E1", Price = 300, OldPrice = 350, InStock = true, Color = "Blue", Size = "Small", Dimensions = "10×10×20", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(3), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
new Product { Name = "VelvetLux Sofa", Description = "Luxurious velvet sofa for modern living rooms", ImageUrl = "F1", Price = 75, OldPrice = null, InStock = true, Color = "Red", Size = "Large", Dimensions = "80×35×40", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "DreamCloud Pillow", Description = "Cooling gel-infused pillow for ultimate comfort", ImageUrl = "A2", Price = 130, OldPrice = 160, InStock = true, Color = "Green", Size = "Large", Dimensions = "22×22×6", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-2), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "PosturePro Chair", Description = "Adjustable chair for long working hours", ImageUrl = "B2", Price = 110, OldPrice = null, InStock = false, Color = "Blue", Size = "Medium", Dimensions = "28×28×38", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-3), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(4), AuctionEndTime = DateTime.UtcNow.AddHours(36), IsAuctionClosed = false },
new Product { Name = "MoonGlow Floor Lamp", Description = "Tall floor lamp with ambient lighting", ImageUrl = "E2", Price = 250, OldPrice = 300, InStock = true, Color = "Black", Size = "Large", Dimensions = "15×15×50", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-1), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Rustic Bookshelf", Description = "Vintage-style bookshelf for home or office", ImageUrl = "F2", Price = 400, OldPrice = null, InStock = true, Color = "Brown", Size = "Large", Dimensions = "40×20×70", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-4), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(5), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
new Product { Name = "Orbit Circular Table", Description = "Compact circular table for small spaces", ImageUrl = "C2", Price = 200, OldPrice = 240, InStock = true, Color = "White", Size = "Medium", Dimensions = "45×45×32", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-5), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Maple Conference Table", Description = "Large maple table for meetings", ImageUrl = "D2", Price = 190, OldPrice = null, InStock = true, Color = "Red", Size = "Large", Dimensions = "65×35×40", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-6), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(6), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
new Product { Name = "PureBliss Pillow", Description = "Hypoallergenic pillow for sensitive sleepers", ImageUrl = "A3", Price = 100, OldPrice = 130, InStock = true, Color = "Beige", Size = "Medium", Dimensions = "20×20×4", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-7), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "RelaxMate Recliner", Description = "Cozy recliner chair with adjustable positions", ImageUrl = "B3", Price = 150, OldPrice = null, InStock = true, Color = "Gray", Size = "Large", Dimensions = "35×35×45", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-8), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(7), AuctionEndTime = DateTime.UtcNow.AddHours(60), IsAuctionClosed = false },
new Product { Name = "Eclipse Circular Table", Description = "Minimalist circular table with metal base", ImageUrl = "C3", Price = 230, OldPrice = 280, InStock = true, Color = "Black", Size = "Small", Dimensions = "36×36×28", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-9), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Walnut Study Table", Description = "Compact walnut table for home offices", ImageUrl = "D3", Price = 160, OldPrice = null, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "48×24×30", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-10), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Twilight Table Lamp", Description = "Sleek table lamp with touch controls", ImageUrl = "E3", Price = 80, OldPrice = 100, InStock = true, Color = "Silver", Size = "Small", Dimensions = "8×8×18", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-11), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(8), AuctionEndTime = DateTime.UtcNow.AddHours(36), IsAuctionClosed = false },
new Product { Name = "UrbanMod Coffee Table", Description = "Modern coffee table with storage", ImageUrl = "F3", Price = 120, OldPrice = null, InStock = true, Color = "White", Size = "Medium", Dimensions = "40×20×18", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-12), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Serenity Pillow", Description = "Orthopedic pillow for neck support", ImageUrl = "A4", Price = 140, OldPrice = 170, InStock = true, Color = "Blue", Size = "Large", Dimensions = "24×24×6", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-13), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(9), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
new Product { Name = "WorkSmart Desk Chair", Description = "Breathable mesh chair for office use", ImageUrl = "B4", Price = 130, OldPrice = null, InStock = true, Color = "Black", Size = "Medium", Dimensions = "26×26×38", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-14), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Nova Circular Table", Description = "Elegant circular table with glass top", ImageUrl = "C4", Price = 250, OldPrice = 300, InStock = true, Color = "Silver", Size = "Medium", Dimensions = "42×42×30", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-15), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(10), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
new Product { Name = "Cherrywood Dining Table", Description = "Classic cherrywood table for large families", ImageUrl = "D4", Price = 200, OldPrice = null, InStock = true, Color = "Red", Size = "Large", Dimensions = "70×35×35", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-16), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Aurora Floor Lamp", Description = "Adjustable floor lamp with warm light", ImageUrl = "E4", Price = 90, OldPrice = 120, InStock = true, Color = "Gold", Size = "Large", Dimensions = "12×12×60", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-17), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "LeatherLux Armchair", Description = "Premium leather armchair for cozy corners", ImageUrl = "F4", Price = 350, OldPrice = null, InStock = true, Color = "Brown", Size = "Large", Dimensions = "34×34×36", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-18), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(11), AuctionEndTime = DateTime.UtcNow.AddHours(60), IsAuctionClosed = false },
new Product { Name = "CozyNest Pillow", Description = "Soft cotton pillow for all-night comfort", ImageUrl = "A5", Price = 110, OldPrice = 140, InStock = true, Color = "Gray", Size = "Medium", Dimensions = "20×20×5", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-19), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "FlexiFit Chair", Description = "Swivel chair with ergonomic design", ImageUrl = "B5", Price = 140, OldPrice = null, InStock = true, Color = "Blue", Size = "Medium", Dimensions = "28×28×40", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-20), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(12), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
new Product { Name = "Stellar Circular Table", Description = "Modern circular table with wooden finish", ImageUrl = "C5", Price = 210, OldPrice = 250, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "40×40×30", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-21), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Teakwood Console Table", Description = "Sleek teakwood table for entryways", ImageUrl = "D5", Price = 170, OldPrice = null, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "50×20×32", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-22), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Dusk Desk Lamp", Description = "Compact desk lamp with USB charging port", ImageUrl = "E5", Price = 70, OldPrice = 90, InStock = true, Color = "Black", Size = "Small", Dimensions = "8×8×15", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-23), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(13), AuctionEndTime = DateTime.UtcNow.AddHours(36), IsAuctionClosed = false },
new Product { Name = "Scandi TV Stand", Description = "Minimalist TV stand with open shelves", ImageUrl = "F5", Price = 180, OldPrice = null, InStock = true, Color = "White", Size = "Large", Dimensions = "60×20×24", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-24), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "LuxeRest Pillow", Description = "Premium pillow with adjustable firmness", ImageUrl = "A6", Price = 150, OldPrice = 180, InStock = true, Color = "White", Size = "Large", Dimensions = "24×24×7", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-25), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(14), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
new Product { Name = "ComfyGlide Rocking Chair", Description = "Smooth rocking chair for relaxation", ImageUrl = "B6", Price = 160, OldPrice = null, InStock = true, Color = "Brown", Size = "Large", Dimensions = "32×32×42", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-26), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "CloudSoft Pillow", Description = "Ultra-soft memory foam pillow for a restful sleep", ImageUrl = "A1", Price = 120, OldPrice = 150, InStock = true, Color = "White", Size = "Medium", Dimensions = "20×20×5", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(1), AuctionEndTime = DateTime.UtcNow.AddHours(24), IsAuctionClosed = false },
new Product { Name = "ErgoComfort Chair", Description = "Ergonomic office chair with lumbar support", ImageUrl = "B1", Price = 95, OldPrice = null, InStock = true, Color = "Black", Size = "Large", Dimensions = "30×30×40", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Luna Circular Table", Description = "Modern circular dining table with a sleek finish", ImageUrl = "C1", Price = 220, OldPrice = 260, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "40×40×30", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(2), AuctionEndTime = DateTime.UtcNow.AddHours(30), IsAuctionClosed = false },
new Product { Name = "Oakwood Dining Table", Description = "Solid oak dining table for family gatherings", ImageUrl = "D1", Price = 180, OldPrice = null, InStock = true, Color = "Green", Size = "Large", Dimensions = "60×30×35", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Starlight Desk Lamp", Description = "Elegant LED desk lamp with adjustable brightness", ImageUrl = "E1", Price = 300, OldPrice = 350, InStock = true, Color = "Blue", Size = "Small", Dimensions = "10×10×20", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(3), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
new Product { Name = "VelvetLux Sofa", Description = "Luxurious velvet sofa for modern living rooms", ImageUrl = "F1", Price = 75, OldPrice = null, InStock = true, Color = "Red", Size = "Large", Dimensions = "80×35×40", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "DreamCloud Pillow", Description = "Cooling gel-infused pillow for ultimate comfort", ImageUrl = "A2", Price = 130, OldPrice = 160, InStock = true, Color = "Green", Size = "Large", Dimensions = "22×22×6", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-2), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "PosturePro Chair", Description = "Adjustable chair for long working hours", ImageUrl = "B2", Price = 110, OldPrice = null, InStock = false, Color = "Blue", Size = "Medium", Dimensions = "28×28×38", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-3), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(4), AuctionEndTime = DateTime.UtcNow.AddHours(36), IsAuctionClosed = false },
new Product { Name = "MoonGlow Floor Lamp", Description = "Tall floor lamp with ambient lighting", ImageUrl = "E2", Price = 250, OldPrice = 300, InStock = true, Color = "Black", Size = "Large", Dimensions = "15×15×50", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-1), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Rustic Bookshelf", Description = "Vintage-style bookshelf for home or office", ImageUrl = "F2", Price = 400, OldPrice = null, InStock = true, Color = "Brown", Size = "Large", Dimensions = "40×20×70", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-4), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(5), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
new Product { Name = "Orbit Circular Table", Description = "Compact circular table for small spaces", ImageUrl = "C2", Price = 200, OldPrice = 240, InStock = true, Color = "White", Size = "Medium", Dimensions = "45×45×32", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-5), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Maple Conference Table", Description = "Large maple table for meetings", ImageUrl = "D2", Price = 190, OldPrice = null, InStock = true, Color = "Red", Size = "Large", Dimensions = "65×35×40", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-6), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(6), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
new Product { Name = "PureBliss Pillow", Description = "Hypoallergenic pillow for sensitive sleepers", ImageUrl = "A3", Price = 100, OldPrice = 130, InStock = true, Color = "Beige", Size = "Medium", Dimensions = "20×20×4", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-7), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "RelaxMate Recliner", Description = "Cozy recliner chair with adjustable positions", ImageUrl = "B3", Price = 150, OldPrice = null, InStock = true, Color = "Gray", Size = "Large", Dimensions = "35×35×45", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-8), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(7), AuctionEndTime = DateTime.UtcNow.AddHours(60), IsAuctionClosed = false },
new Product { Name = "Eclipse Circular Table", Description = "Minimalist circular table with metal base", ImageUrl = "C3", Price = 230, OldPrice = 280, InStock = true, Color = "Black", Size = "Small", Dimensions = "36×36×28", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-9), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Walnut Study Table", Description = "Compact walnut table for home offices", ImageUrl = "D3", Price = 160, OldPrice = null, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "48×24×30", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-10), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Twilight Table Lamp", Description = "Sleek table lamp with touch controls", ImageUrl = "E3", Price = 80, OldPrice = 100, InStock = true, Color = "Silver", Size = "Small", Dimensions = "8×8×18", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-11), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(8), AuctionEndTime = DateTime.UtcNow.AddHours(36), IsAuctionClosed = false },
new Product { Name = "UrbanMod Coffee Table", Description = "Modern coffee table with storage", ImageUrl = "F3", Price = 120, OldPrice = null, InStock = true, Color = "White", Size = "Medium", Dimensions = "40×20×18", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-12), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Serenity Pillow", Description = "Orthopedic pillow for neck support", ImageUrl = "A4", Price = 140, OldPrice = 170, InStock = true, Color = "Blue", Size = "Large", Dimensions = "24×24×6", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-13), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(9), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
new Product { Name = "WorkSmart Desk Chair", Description = "Breathable mesh chair for office use", ImageUrl = "B4", Price = 130, OldPrice = null, InStock = true, Color = "Black", Size = "Medium", Dimensions = "26×26×38", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-14), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Nova Circular Table", Description = "Elegant circular table with glass top", ImageUrl = "C4", Price = 250, OldPrice = 300, InStock = true, Color = "Silver", Size = "Medium", Dimensions = "42×42×30", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-15), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(10), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
new Product { Name = "Cherrywood Dining Table", Description = "Classic cherrywood table for large families", ImageUrl = "D4", Price = 200, OldPrice = null, InStock = true, Color = "Red", Size = "Large", Dimensions = "70×35×35", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-16), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Aurora Floor Lamp", Description = "Adjustable floor lamp with warm light", ImageUrl = "E4", Price = 90, OldPrice = 120, InStock = true, Color = "Gold", Size = "Large", Dimensions = "12×12×60", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-17), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "LeatherLux Armchair", Description = "Premium leather armchair for cozy corners", ImageUrl = "F4", Price = 350, OldPrice = null, InStock = true, Color = "Brown", Size = "Large", Dimensions = "34×34×36", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-18), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(11), AuctionEndTime = DateTime.UtcNow.AddHours(60), IsAuctionClosed = false },
new Product { Name = "CozyNest Pillow", Description = "Soft cotton pillow for all-night comfort", ImageUrl = "A5", Price = 110, OldPrice = 140, InStock = true, Color = "Gray", Size = "Medium", Dimensions = "20×20×5", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-19), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "FlexiFit Chair", Description = "Swivel chair with ergonomic design", ImageUrl = "B5", Price = 140, OldPrice = null, InStock = true, Color = "Blue", Size = "Medium", Dimensions = "28×28×40", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-20), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(12), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
new Product { Name = "Stellar Circular Table", Description = "Modern circular table with wooden finish", ImageUrl = "C5", Price = 210, OldPrice = 250, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "40×40×30", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-21), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Teakwood Console Table", Description = "Sleek teakwood table for entryways", ImageUrl = "D5", Price = 170, OldPrice = null, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "50×20×32", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-22), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Dusk Desk Lamp", Description = "Compact desk lamp with USB charging port", ImageUrl = "E5", Price = 70, OldPrice = 90, InStock = true, Color = "Black", Size = "Small", Dimensions = "8×8×15", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-23), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(13), AuctionEndTime = DateTime.UtcNow.AddHours(36), IsAuctionClosed = false },
new Product { Name = "Scandi TV Stand", Description = "Minimalist TV stand with open shelves", ImageUrl = "F5", Price = 180, OldPrice = null, InStock = true, Color = "White", Size = "Large", Dimensions = "60×20×24", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-24), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "LuxeRest Pillow", Description = "Premium pillow with adjustable firmness", ImageUrl = "A6", Price = 150, OldPrice = 180, InStock = true, Color = "White", Size = "Large", Dimensions = "24×24×7", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-25), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(14), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
new Product { Name = "ComfyGlide Rocking Chair", Description = "Smooth rocking chair for relaxation", ImageUrl = "B6", Price = 160, OldPrice = null, InStock = true, Color = "Brown", Size = "Large", Dimensions = "32×32×42", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-26), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Zen Circular Table", Description = "Simple circular table with oak finish", ImageUrl = "C6", Price = 190, OldPrice = 230, InStock = true, Color = "Beige", Size = "Small", Dimensions = "38×38×29", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-27), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(15), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
new Product { Name = "Mahogany Dining Table", Description = "Elegant mahogany table for formal dining", ImageUrl = "D6", Price = 220, OldPrice = null, InStock = true, Color = "Brown", Size = "Large", Dimensions = "72×36×35", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-28), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Solar Table Lamp", Description = "Modern lamp with energy-efficient LED", ImageUrl = "E6", Price = 85, OldPrice = 110, InStock = true, Color = "White", Size = "Small", Dimensions = "9×9×16", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-29), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(16), AuctionEndTime = DateTime.UtcNow.AddHours(36), IsAuctionClosed = false },
new Product { Name = "Nordic Side Table", Description = "Compact side table with minimalist design", ImageUrl = "F6", Price = 100, OldPrice = null, InStock = true, Color = "Gray", Size = "Small", Dimensions = "20×20×22", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-30), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Tranquil Pillow", Description = "Breathable pillow for cool sleep", ImageUrl = "A7", Price = 120, OldPrice = 150, InStock = true, Color = "Blue", Size = "Medium", Dimensions = "22×22×5", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-31), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(17), AuctionEndTime = DateTime.UtcNow.AddHours(60), IsAuctionClosed = false },
new Product { Name = "EasePro Office Chair", Description = "Adjustable chair with breathable fabric", ImageUrl = "B7", Price = 145, OldPrice = null, InStock = true, Color = "Black", Size = "Medium", Dimensions = "27×27×39", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-32), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Aurora Circular Table", Description = "Stylish circular table with marble top", ImageUrl = "C7", Price = 260, OldPrice = 310, InStock = true, Color = "White", Size = "Medium", Dimensions = "44×44×31", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-33), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(18), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
new Product { Name = "Bamboo Work Desk", Description = "Eco-friendly bamboo desk for small spaces", ImageUrl = "D7", Price = 175, OldPrice = null, InStock = true, Color = "Natural", Size = "Medium", Dimensions = "48×24×30", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-34), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Lunar Floor Lamp", Description = "Curved floor lamp with soft glow", ImageUrl = "E7", Price = 95, OldPrice = 125, InStock = true, Color = "Silver", Size = "Large", Dimensions = "14×14×62", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-35), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(19), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
new Product { Name = "Vintage Rocking Chair", Description = "Classic wooden rocking chair for porches", ImageUrl = "F7", Price = 200, OldPrice = null, InStock = true, Color = "Brown", Size = "Large", Dimensions = "30×30×40", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-36), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "SoftTouch Pillow", Description = "Plush pillow for extra comfort", ImageUrl = "A8", Price = 115, OldPrice = 145, InStock = true, Color = "Gray", Size = "Large", Dimensions = "23×23×6", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-37), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(20), AuctionEndTime = DateTime.UtcNow.AddHours(60), IsAuctionClosed = false },
new Product { Name = "BalanceFit Chair", Description = "Ergonomic chair with tilt mechanism", ImageUrl = "B8", Price = 155, OldPrice = null, InStock = true, Color = "Blue", Size = "Medium", Dimensions = "28×28×41", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-38), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Comet Circular Table", Description = "Compact circular table with modern design", ImageUrl = "C8", Price = 205, OldPrice = 245, InStock = true, Color = "Black", Size = "Small", Dimensions = "36×36×30", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-39), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(21), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
new Product { Name = "Pinewood Coffee Table", Description = "Rustic coffee table with natural finish", ImageUrl = "D8", Price = 165, OldPrice = null, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "42×22×18", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-40), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Glow Table Lamp", Description = "Small lamp with colorful LED options", ImageUrl = "E8", Price = 75, OldPrice = 95, InStock = true, Color = "Multicolor", Size = "Small", Dimensions = "7×7×14", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-41), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(23),  AuctionEndTime = DateTime.UtcNow.AddHours(36), IsAuctionClosed = false },
new Product { Name = "Modern Bookshelf", Description = "Sleek bookshelf with asymmetrical design", ImageUrl = "F8", Price = 190, OldPrice = null, InStock = true, Color = "White", Size = "Large", Dimensions = "36×12×60", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-42), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "DreamWeave Pillow", Description = "Lightweight pillow for side sleepers", ImageUrl = "A9", Price = 125, OldPrice = 155, InStock = true, Color = "Beige", Size = "Medium", Dimensions = "21×21×5", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-43), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(23), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
new Product { Name = "SwiftMove Chair", Description = "Mobile chair with smooth casters", ImageUrl = "B9", Price = 150, OldPrice = null, InStock = true, Color = "Gray", Size = "Medium", Dimensions = "26×26×38", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-44), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Meteor Circular Table", Description = "Bold circular table with metal frame", ImageUrl = "C9", Price = 215, OldPrice = 260, InStock = true, Color = "Silver", Size = "Medium", Dimensions = "40×40×31", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-45), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(24), AuctionEndTime = DateTime.UtcNow.AddHours(60), IsAuctionClosed = false },
new Product { Name = "Cedarwood Side Table", Description = "Small side table with rustic charm", ImageUrl = "D9", Price = 160, OldPrice = null, InStock = true, Color = "Brown", Size = "Small", Dimensions = "18×18×20", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-46), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Dawn Floor Lamp", Description = "Tall lamp with adjustable arm", ImageUrl = "E9", Price = 100, OldPrice = 130, InStock = true, Color = "Black", Size = "Large", Dimensions = "15×15×65", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-47), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(25), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
new Product { Name = "EcoChic Ottoman", Description = "Versatile ottoman with hidden storage", ImageUrl = "F9", Price = 170, OldPrice = null, InStock = true, Color = "Green", Size = "Medium", Dimensions = "24×24×16", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-48), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Blissful Pillow", Description = "Soft pillow with cooling technology", ImageUrl = "A10", Price = 130, OldPrice = 160, InStock = true, Color = "White", Size = "Large", Dimensions = "22×22×6", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-49), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(26), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
new Product { Name = "VitaFit Desk Chair", Description = "Ergonomic chair with mesh back", ImageUrl = "B10", Price = 145, OldPrice = null, InStock = true, Color = "Black", Size = "Medium", Dimensions = "27×27×40", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-50), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Cosmo Circular Table", Description = "Modern circular table with glossy finish", ImageUrl = "C10", Price = 225, OldPrice = 270, InStock = true, Color = "White", Size = "Medium", Dimensions = "42×42×30", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-51), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(27), AuctionEndTime = DateTime.UtcNow.AddHours(60), IsAuctionClosed = false },
new Product { Name = "Elmwood Dining Table", Description = "Spacious elmwood table for gatherings", ImageUrl = "D10", Price = 210, OldPrice = null, InStock = true, Color = "Brown", Size = "Large", Dimensions = "70×35×35", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-52), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Sparkle Desk Lamp", Description = "Compact lamp with crystal accents", ImageUrl = "E10", Price = 85, OldPrice = 110, InStock = true, Color = "Silver", Size = "Small", Dimensions = "8×8×16", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-53), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(28), AuctionEndTime = DateTime.UtcNow.AddHours(36), IsAuctionClosed = false },
new Product { Name = "Retro Coffee Table", Description = "Vintage-inspired coffee table", ImageUrl = "F10", Price = 150, OldPrice = null, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "40×20×18", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-54), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Harmony Pillow", Description = "Balanced pillow for all sleep positions", ImageUrl = "A11", Price = 140, OldPrice = 170, InStock = true, Color = "Blue", Size = "Large", Dimensions = "24×24×6", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-55), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(29), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
new Product { Name = "GlidePro Recliner", Description = "Smooth recliner with footrest", ImageUrl = "B11", Price = 160, OldPrice = null, InStock = true, Color = "Gray", Size = "Large", Dimensions = "34×34×42", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-56), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Orbit Circular Table", Description = "Sleek circular table with chrome base", ImageUrl = "C11", Price = 235, OldPrice = 280, InStock = true, Color = "Silver", Size = "Medium", Dimensions = "44×44×31", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-57), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(30), AuctionEndTime = DateTime.UtcNow.AddHours(60), IsAuctionClosed = false },
new Product { Name = "Birchwood Console Table", Description = "Elegant console table for hallways", ImageUrl = "D11", Price = 180, OldPrice = null, InStock = true, Color = "White", Size = "Medium", Dimensions = "50×20×32", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-58), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Radiance Floor Lamp", Description = "Tall lamp with dimmable light", ImageUrl = "E11", Price = 95, OldPrice = 125, InStock = true, Color = "Gold", Size = "Large", Dimensions = "14×14×62", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-59), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(31), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
new Product { Name = "LuxeVelvet Sofa", Description = "Plush velvet sofa for cozy living", ImageUrl = "F11", Price = 400, OldPrice = null, InStock = true, Color = "Blue", Size = "Large", Dimensions = "80×35×40", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-60), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Restful Pillow", Description = "Memory foam pillow for deep sleep", ImageUrl = "A12", Price = 135, OldPrice = 165, InStock = true, Color = "Gray", Size = "Large", Dimensions = "23×23×6", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-61), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(32), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
new Product { Name = "ErgoPlus Chair", Description = "Adjustable chair with lumbar cushion", ImageUrl = "B12", Price = 155, OldPrice = null, InStock = true, Color = "Black", Size = "Medium", Dimensions = "28×28×41", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-62), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Nebula Circular Table", Description = "Modern circular table with glass top", ImageUrl = "C12", Price = 245, OldPrice = 290, InStock = true, Color = "Silver", Size = "Medium", Dimensions = "42×42×30", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-63), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(33), AuctionEndTime = DateTime.UtcNow.AddHours(60), IsAuctionClosed = false },
new Product { Name = "Ashwood Dining Table", Description = "Sturdy ashwood table for large meals", ImageUrl = "D12", Price = 200, OldPrice = null, InStock = true, Color = "Brown", Size = "Large", Dimensions = "72×36×35", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-64), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Shimmer Table Lamp", Description = "Elegant lamp with metallic finish", ImageUrl = "E12", Price = 80, OldPrice = 100, InStock = true, Color = "Silver", Size = "Small", Dimensions = "8×8×18", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-65), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(34), AuctionEndTime = DateTime.UtcNow.AddHours(36), IsAuctionClosed = false },
new Product { Name = "MidCentury Bookshelf", Description = "Retro bookshelf with clean lines", ImageUrl = "F12", Price = 220, OldPrice = null, InStock = true, Color = "Brown", Size = "Large", Dimensions = "40×12×60", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-66), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "CalmCloud Pillow", Description = "Soft pillow with hypoallergenic fill", ImageUrl = "A13", Price = 145, OldPrice = 175, InStock = true, Color = "White", Size = "Large", Dimensions = "24×24×6", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-67), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(35), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
new Product { Name = "RelaxPro Recliner", Description = "Comfortable recliner with plush cushioning", ImageUrl = "B13", Price = 170, OldPrice = null, InStock = true, Color = "Gray", Size = "Large", Dimensions = "34×34×42", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-68), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Galaxy Circular Table", Description = "Sleek circular table with wooden top", ImageUrl = "C13", Price = 255, OldPrice = 300, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "44×44×31", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-69), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(36), AuctionEndTime = DateTime.UtcNow.AddHours(60), IsAuctionClosed = false },
new Product { Name = "Rosewood Console Table", Description = "Elegant rosewood table for entryways", ImageUrl = "D13", Price = 190, OldPrice = null, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "50×20×32", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-70), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Glow Floor Lamp", Description = "Modern floor lamp with adjustable brightness", ImageUrl = "E13", Price = 100, OldPrice = 130, InStock = true, Color = "Silver", Size = "Large", Dimensions = "14×14×62", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-71), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(37), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
new Product { Name = "Scandi Armchair", Description = "Minimalist armchair with wooden frame", ImageUrl = "F13", Price = 180, OldPrice = null, InStock = true, Color = "Gray", Size = "Medium", Dimensions = "30×30×36", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-72), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "PureComfort Pillow", Description = "Soft pillow with memory foam core", ImageUrl = "A14", Price = 150, OldPrice = 180, InStock = true, Color = "Blue", Size = "Large", Dimensions = "24×24×6", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-73), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(38), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
new Product { Name = "FlexPro Desk Chair", Description = "Ergonomic chair with adjustable arms", ImageUrl = "B14", Price = 165, OldPrice = null, InStock = true, Color = "Black", Size = "Medium", Dimensions = "28×28×41", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-74), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Star Circular Table", Description = "Modern circular table with metal legs", ImageUrl = "C14", Price = 265, OldPrice = 310, InStock = true, Color = "White", Size = "Medium", Dimensions = "44×44×31", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-75), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(39), AuctionEndTime = DateTime.UtcNow.AddHours(60), IsAuctionClosed = false },
new Product { Name = "Oakwood Side Table", Description = "Compact side table with drawer", ImageUrl = "D14", Price = 175, OldPrice = null, InStock = true, Color = "Brown", Size = "Small", Dimensions = "20×20×22", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-76), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Luster Table Lamp", Description = "Sleek lamp with touch dimmer", ImageUrl = "E14", Price = 90, OldPrice = 120, InStock = true, Color = "Gold", Size = "Small", Dimensions = "8×8×18", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-77), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(40), AuctionEndTime = DateTime.UtcNow.AddHours(36), IsAuctionClosed = false },
new Product { Name = "Rustic Coffee Table", Description = "Wooden coffee table with distressed finish", ImageUrl = "F14", Price = 190, OldPrice = null, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "40×20×18", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-78), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "CloudSoft Pillow", Description = "Ultra-soft memory foam pillow for a restful sleep", ImageUrl = "A1", Price = 120, OldPrice = 150, InStock = true, Color = "White", Size = "Medium", Dimensions = "20×20×5", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(1), AuctionEndTime = DateTime.UtcNow.AddHours(24), IsAuctionClosed = false },
new Product { Name = "ErgoComfort Chair", Description = "Ergonomic office chair with lumbar support", ImageUrl = "B1", Price = 95, OldPrice = null, InStock = true, Color = "Black", Size = "Large", Dimensions = "30×30×40", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Luna Circular Table", Description = "Modern circular dining table with a sleek finish", ImageUrl = "C1", Price = 220, OldPrice = 260, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "40×40×30", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(2), AuctionEndTime = DateTime.UtcNow.AddHours(30), IsAuctionClosed = false },
new Product { Name = "Oakwood Dining Table", Description = "Solid oak dining table for family gatherings", ImageUrl = "D1", Price = 180, OldPrice = null, InStock = true, Color = "Green", Size = "Large", Dimensions = "60×30×35", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Starlight Desk Lamp", Description = "Elegant LED desk lamp with adjustable brightness", ImageUrl = "E1", Price = 300, OldPrice = 350, InStock = true, Color = "Blue", Size = "Small", Dimensions = "10×10×20", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(3), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
new Product { Name = "VelvetLux Sofa", Description = "Luxurious velvet sofa for modern living rooms", ImageUrl = "F1", Price = 75, OldPrice = null, InStock = true, Color = "Red", Size = "Large", Dimensions = "80×35×40", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "DreamCloud Pillow", Description = "Cooling gel-infused pillow for ultimate comfort", ImageUrl = "A2", Price = 130, OldPrice = 160, InStock = true, Color = "Green", Size = "Large", Dimensions = "22×22×6", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-2), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "PosturePro Chair", Description = "Adjustable chair for long working hours", ImageUrl = "B2", Price = 110, OldPrice = null, InStock = false, Color = "Blue", Size = "Medium", Dimensions = "28×28×38", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-3), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(4), AuctionEndTime = DateTime.UtcNow.AddHours(36), IsAuctionClosed = false },
new Product { Name = "MoonGlow Floor Lamp", Description = "Tall floor lamp with ambient lighting", ImageUrl = "E2", Price = 250, OldPrice = 300, InStock = true, Color = "Black", Size = "Large", Dimensions = "15×15×50", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-1), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Rustic Bookshelf", Description = "Vintage-style bookshelf for home or office", ImageUrl = "F2", Price = 400, OldPrice = null, InStock = true, Color = "Brown", Size = "Large", Dimensions = "40×20×70", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-4), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(5), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
new Product { Name = "Orbit Circular Table", Description = "Compact circular table for small spaces", ImageUrl = "C2", Price = 200, OldPrice = 240, InStock = true, Color = "White", Size = "Medium", Dimensions = "45×45×32", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-5), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
new Product { Name = "Maple Conference Table", Description = "Large maple table for meetings", ImageUrl = "D2", Price = 190, OldPrice = null, InStock = true, Color = "Red", Size = "Large", Dimensions = "65×35×40", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-6), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(6), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
new Product { Name = "CloudSoft Pillow", Description = "Ultra-soft memory foam pillow for a restful sleep", ImageUrl = "A1", Price = 120, OldPrice = 150, InStock = true, Color = "White", Size = "Medium", Dimensions = "20×20×5", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(1), AuctionEndTime = DateTime.UtcNow.AddHours(24), IsAuctionClosed = false },
            new Product { Name = "ErgoComfort Chair", Description = "Ergonomic office chair with lumbar support", ImageUrl = "B1", Price = 95, OldPrice = null, InStock = true, Color = "Black", Size = "Large", Dimensions = "30×30×40", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Luna Circular Table", Description = "Modern circular dining table with a sleek finish", ImageUrl = "C1", Price = 220, OldPrice = 260, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "40×40×30", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(2), AuctionEndTime = DateTime.UtcNow.AddHours(30), IsAuctionClosed = false },
            new Product { Name = "Oakwood Dining Table", Description = "Solid oak dining table for family gatherings", ImageUrl = "D1", Price = 180, OldPrice = null, InStock = true, Color = "Green", Size = "Large", Dimensions = "60×30×35", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Starlight Desk Lamp", Description = "Elegant LED desk lamp with adjustable brightness", ImageUrl = "E1", Price = 300, OldPrice = 350, InStock = true, Color = "Blue", Size = "Small", Dimensions = "10×10×20", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(3), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
            new Product { Name = "VelvetLux Sofa", Description = "Luxurious velvet sofa for modern living rooms", ImageUrl = "F1", Price = 75, OldPrice = null, InStock = true, Color = "Red", Size = "Large", Dimensions = "80×35×40", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow, UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "DreamCloud Pillow", Description = "Cooling gel-infused pillow for ultimate comfort", ImageUrl = "A2", Price = 130, OldPrice = 160, InStock = true, Color = "Green", Size = "Large", Dimensions = "22×22×6", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-2), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "PosturePro Chair", Description = "Adjustable chair for long working hours", ImageUrl = "B2", Price = 110, OldPrice = null, InStock = false, Color = "Blue", Size = "Medium", Dimensions = "28×28×38", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-3), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(4), AuctionEndTime = DateTime.UtcNow.AddHours(36), IsAuctionClosed = false },
            new Product { Name = "MoonGlow Floor Lamp", Description = "Tall floor lamp with ambient lighting", ImageUrl = "E2", Price = 250, OldPrice = 300, InStock = true, Color = "Black", Size = "Large", Dimensions = "15×15×50", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-1), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Rustic Bookshelf", Description = "Vintage-style bookshelf for home or office", ImageUrl = "F2", Price = 400, OldPrice = null, InStock = true, Color = "Brown", Size = "Large", Dimensions = "40×20×70", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-4), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(5), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
            new Product { Name = "Orbit Circular Table", Description = "Compact circular table for small spaces", ImageUrl = "C2", Price = 200, OldPrice = 240, InStock = true, Color = "White", Size = "Medium", Dimensions = "45×45×32", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-5), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Maple Conference Table", Description = "Large maple table for meetings", ImageUrl = "D2", Price = 190, OldPrice = null, InStock = true, Color = "Red", Size = "Large", Dimensions = "65×35×40", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-6), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(6), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
            new Product { Name = "PureBliss Pillow", Description = "Hypoallergenic pillow for sensitive sleepers", ImageUrl = "A3", Price = 100, OldPrice = 130, InStock = true, Color = "Beige", Size = "Medium", Dimensions = "20×20×4", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-7), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "RelaxMate Recliner", Description = "Cozy recliner chair with adjustable positions", ImageUrl = "B3", Price = 150, OldPrice = null, InStock = true, Color = "Gray", Size = "Large", Dimensions = "35×35×45", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-8), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(7), AuctionEndTime = DateTime.UtcNow.AddHours(60), IsAuctionClosed = false },
            new Product { Name = "Eclipse Circular Table", Description = "Minimalist circular table with metal base", ImageUrl = "C3", Price = 230, OldPrice = 280, InStock = true, Color = "Black", Size = "Small", Dimensions = "36×36×28", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-9), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Walnut Study Table", Description = "Compact walnut table for home offices", ImageUrl = "D3", Price = 160, OldPrice = null, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "48×24×30", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-10), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Twilight Table Lamp", Description = "Sleek table lamp with touch controls", ImageUrl = "E3", Price = 80, OldPrice = 100, InStock = true, Color = "Silver", Size = "Small", Dimensions = "8×8×18", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-11), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(8), AuctionEndTime = DateTime.UtcNow.AddHours(36), IsAuctionClosed = false },
            new Product { Name = "UrbanMod Coffee Table", Description = "Modern coffee table with storage", ImageUrl = "F3", Price = 120, OldPrice = null, InStock = true, Color = "White", Size = "Medium", Dimensions = "40×20×18", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-12), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Serenity Pillow", Description = "Orthopedic pillow for neck support", ImageUrl = "A4", Price = 140, OldPrice = 170, InStock = true, Color = "Blue", Size = "Large", Dimensions = "24×24×6", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-13), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(9), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
            new Product { Name = "WorkSmart Desk Chair", Description = "Breathable mesh chair for office use", ImageUrl = "B4", Price = 130, OldPrice = null, InStock = true, Color = "Black", Size = "Medium", Dimensions = "26×26×38", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-14), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Nova Circular Table", Description = "Elegant circular table with glass top", ImageUrl = "C4", Price = 250, OldPrice = 300, InStock = true, Color = "Silver", Size = "Medium", Dimensions = "42×42×30", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-15), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(10), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
            new Product { Name = "Cherrywood Dining Table", Description = "Classic cherrywood table for large families", ImageUrl = "D4", Price = 200, OldPrice = null, InStock = true, Color = "Red", Size = "Large", Dimensions = "70×35×35", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-16), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Aurora Floor Lamp", Description = "Adjustable floor lamp with warm light", ImageUrl = "E4", Price = 90, OldPrice = 120, InStock = true, Color = "Gold", Size = "Large", Dimensions = "12×12×60", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-17), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "LeatherLux Armchair", Description = "Premium leather armchair for cozy corners", ImageUrl = "F4", Price = 350, OldPrice = null, InStock = true, Color = "Brown", Size = "Large", Dimensions = "34×34×36", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-18), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(11), AuctionEndTime = DateTime.UtcNow.AddHours(60), IsAuctionClosed = false,  },
            new Product { Name = "CozyNest Pillow", Description = "Soft cotton pillow for all-night comfort", ImageUrl = "A5", Price = 110, OldPrice = 140, InStock = true, Color = "Gray", Size = "Medium", Dimensions = "20×20×5", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-19), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "FlexiFit Chair", Description = "Swivel chair with ergonomic design", ImageUrl = "B5", Price = 140, OldPrice = null, InStock = true, Color = "Blue", Size = "Medium", Dimensions = "28×28×40", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-20), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(12), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
            new Product { Name = "Stellar Circular Table", Description = "Modern circular table with wooden finish", ImageUrl = "C5", Price = 210, OldPrice = 250, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "40×40×30", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-21), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Teakwood Console Table", Description = "Sleek teakwood table for entryways", ImageUrl = "D5", Price = 170, OldPrice = null, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "50×20×32", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-22), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Dusk Desk Lamp", Description = "Compact desk lamp with USB charging port", ImageUrl = "E5", Price = 70, OldPrice = 90, InStock = true, Color = "Black", Size = "Small", Dimensions = "8×8×15", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-23), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(13), AuctionEndTime = DateTime.UtcNow.AddHours(36), IsAuctionClosed = false },
            new Product { Name = "Scandi TV Stand", Description = "Minimalist TV stand with open shelves", ImageUrl = "F5", Price = 180, OldPrice = null, InStock = true, Color = "White", Size = "Large", Dimensions = "60×20×24", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-24), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "LuxeRest Pillow", Description = "Premium pillow with adjustable firmness", ImageUrl = "A6", Price = 150, OldPrice = 180, InStock = true, Color = "White", Size = "Large", Dimensions = "24×24×7", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-25), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(14), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
            new Product { Name = "ComfyGlide Rocking Chair", Description = "Smooth rocking chair for relaxation", ImageUrl = "B6", Price = 160, OldPrice = null, InStock = true, Color = "Brown", Size = "Large", Dimensions = "32×32×42", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-26), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Zen Circular Table", Description = "Simple circular table with oak finish", ImageUrl = "C6", Price = 190, OldPrice = 230, InStock = true, Color = "Beige", Size = "Small", Dimensions = "38×38×29", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-27), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(15), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
            new Product { Name = "Mahogany Dining Table", Description = "Elegant mahogany table for formal dining", ImageUrl = "D6", Price = 220, OldPrice = null, InStock = true, Color = "Brown", Size = "Large", Dimensions = "72×36×35", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-28), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Solar Table Lamp", Description = "Modern lamp with energy-efficient LED", ImageUrl = "E6", Price = 85, OldPrice = 110, InStock = true, Color = "White", Size = "Small", Dimensions = "9×9×16", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-29), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(16), AuctionEndTime = DateTime.UtcNow.AddHours(36), IsAuctionClosed = false },
            new Product { Name = "Nordic Side Table", Description = "Compact side table with minimalist design", ImageUrl = "F6", Price = 100, OldPrice = null, InStock = true, Color = "Gray", Size = "Small", Dimensions = "20×20×22", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-30), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Tranquil Pillow", Description = "Breathable pillow for cool sleep", ImageUrl = "A7", Price = 120, OldPrice = 150, InStock = true, Color = "Blue", Size = "Medium", Dimensions = "22×22×5", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-31), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(17), AuctionEndTime = DateTime.UtcNow.AddHours(60), IsAuctionClosed = false },
            new Product { Name = "EasePro Office Chair", Description = "Adjustable chair with breathable fabric", ImageUrl = "B7", Price = 145, OldPrice = null, InStock = true, Color = "Black", Size = "Medium", Dimensions = "27×27×39", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-32), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Aurora Circular Table", Description = "Stylish circular table with marble top", ImageUrl = "C7", Price = 260, OldPrice = 310, InStock = true, Color = "White", Size = "Medium", Dimensions = "44×44×31", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-33), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(18), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
            new Product { Name = "Bamboo Work Desk", Description = "Eco-friendly bamboo desk for small spaces", ImageUrl = "D7", Price = 175, OldPrice = null, InStock = true, Color = "Natural", Size = "Medium", Dimensions = "48×24×30", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-34), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Lunar Floor Lamp", Description = "Curved floor lamp with soft glow", ImageUrl = "E7", Price = 95, OldPrice = 125, InStock = true, Color = "Silver", Size = "Large", Dimensions = "14×14×62", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-35), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(19), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
            new Product { Name = "Vintage Rocking Chair", Description = "Classic wooden rocking chair for porches", ImageUrl = "F7", Price = 200, OldPrice = null, InStock = true, Color = "Brown", Size = "Large", Dimensions = "30×30×40", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-36), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "SoftTouch Pillow", Description = "Plush pillow for extra comfort", ImageUrl = "A8", Price = 115, OldPrice = 145, InStock = true, Color = "Gray", Size = "Large", Dimensions = "23×23×6", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-37), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(20), AuctionEndTime = DateTime.UtcNow.AddHours(60), IsAuctionClosed = false },
            new Product { Name = "BalanceFit Chair", Description = "Ergonomic chair with tilt mechanism", ImageUrl = "B8", Price = 155, OldPrice = null, InStock = true, Color = "Blue", Size = "Medium", Dimensions = "28×28×41", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-38), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Comet Circular Table", Description = "Compact circular table with modern design", ImageUrl = "C8", Price = 205, OldPrice = 245, InStock = true, Color = "Black", Size = "Small", Dimensions = "36×36×30", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-39), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(21), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
            new Product { Name = "Pinewood Coffee Table", Description = "Rustic coffee table with natural finish", ImageUrl = "D8", Price = 165, OldPrice = null, InStock = true, Color = "Brown", Size = "Medium", Dimensions = "42×22×18", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-40), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Glow Table Lamp", Description = "Small lamp with colorful LED options", ImageUrl = "E8", Price = 75, OldPrice = 95, InStock = true, Color = "Multicolor", Size = "Small", Dimensions = "7×7×14", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-41), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(22), AuctionEndTime = DateTime.UtcNow.AddHours(36), IsAuctionClosed = false },
            new Product { Name = "Modern Bookshelf", Description = "Sleek bookshelf with asymmetrical design", ImageUrl = "F8", Price = 190, OldPrice = null, InStock = true, Color = "White", Size = "Large", Dimensions = "36×12×60", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-42), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "DreamWeave Pillow", Description = "Lightweight pillow for side sleepers", ImageUrl = "A9", Price = 125, OldPrice = 155, InStock = true, Color = "Beige", Size = "Medium", Dimensions = "21×21×5", ProductCategoryId = 1, CreatedAt = DateTime.UtcNow.AddDays(-43), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(23), AuctionEndTime = DateTime.UtcNow.AddHours(72), IsAuctionClosed = false },
            new Product { Name = "SwiftMove Chair", Description = "Mobile chair with smooth casters", ImageUrl = "B9", Price = 150, OldPrice = null, InStock = true, Color = "Gray", Size = "Medium", Dimensions = "26×26×38", ProductCategoryId = 2, CreatedAt = DateTime.UtcNow.AddDays(-44), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Meteor Circular Table", Description = "Bold circular table with metal frame", ImageUrl = "C9", Price = 215, OldPrice = 260, InStock = true, Color = "Silver", Size = "Medium", Dimensions = "40×40×31", ProductCategoryId = 3, CreatedAt = DateTime.UtcNow.AddDays(-45), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(24), AuctionEndTime = DateTime.UtcNow.AddHours(60), IsAuctionClosed = false },
            new Product { Name = "Cedarwood Side Table", Description = "Small side table with rustic charm", ImageUrl = "D9", Price = 160, OldPrice = null, InStock = true, Color = "Brown", Size = "Small", Dimensions = "18×18×20", ProductCategoryId = 4, CreatedAt = DateTime.UtcNow.AddDays(-46), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },
            new Product { Name = "Dawn Floor Lamp", Description = "Tall lamp with adjustable arm", ImageUrl = "E9", Price = 100, OldPrice = 130, InStock = true, Color = "Black", Size = "Large", Dimensions = "15×15×65", ProductCategoryId = 5, CreatedAt = DateTime.UtcNow.AddDays(-47), UserCreatedId = string.Empty, IsAuction = true, AuctionStartTime = DateTime.UtcNow.AddHours(25), AuctionEndTime = DateTime.UtcNow.AddHours(48), IsAuctionClosed = false },
            new Product { Name = "EcoChic Ottoman", Description = "Versatile ottoman with hidden storage", ImageUrl = "F9", Price = 170, OldPrice = null, InStock = true, Color = "Green", Size = "Medium", Dimensions = "24×24×16", ProductCategoryId = 6, CreatedAt = DateTime.UtcNow.AddDays(-48), UserCreatedId = string.Empty, IsAuction = false, AuctionStartTime = null, AuctionEndTime = null, IsAuctionClosed = false },


            new Product
            {
                Name = "Product 1",
                Description = "Description for product 1",
                ImageUrl = "A1",
                Price = 120,
                OldPrice = 150,
                InStock = true,
                Color = "White",
                Size = "Medium",
                Dimensions = "20×20×5",
                ProductCategoryId = 1,
                CreatedAt = DateTime.UtcNow,
                UserCreatedId = string.Empty,
                IsAuction = true,
                AuctionStartTime = DateTime.UtcNow.AddHours(1),
                AuctionEndTime = DateTime.UtcNow.AddHours(24),
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "Product 2",
                Description = "Description for product 2",
                ImageUrl = "B1",
                Price = 95,
                OldPrice = null,
                InStock = true,
                Color = "Black",
                Size = "Large",
                Dimensions = "30×30×40",
                ProductCategoryId = 2,
                CreatedAt = DateTime.UtcNow,
                UserCreatedId = string.Empty,
                IsAuction = false,
                AuctionStartTime = null,
                AuctionEndTime = null,
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "Product 3",
                Description = "Description for product 3",
                ImageUrl = "C1",
                Price = 220,
                OldPrice = 260,
                InStock = true,
                Color = "Brown",
                Size = "Medium",
                Dimensions = "40×40×30",
                ProductCategoryId = 3,
                CreatedAt = DateTime.UtcNow,
                UserCreatedId = string.Empty,
                IsAuction = true,
                AuctionStartTime = DateTime.UtcNow.AddHours(2),
                AuctionEndTime = DateTime.UtcNow.AddHours(30),
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "Product 4",
                Description = "Description for product 4",
                ImageUrl = "D1",
                Price = 180,
                OldPrice = null,
                InStock = true,
                Color = "Green",
                Size = "Large",
                Dimensions = "60×30×35",
                ProductCategoryId = 4,
                CreatedAt = DateTime.UtcNow,
                UserCreatedId = string.Empty,
                IsAuction = false,
                AuctionStartTime = null,
                AuctionEndTime = null,
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "Product 5",
                Description = "Description for product 5",
                ImageUrl = "E1",
                Price = 300,
                OldPrice = 350,
                InStock = true,
                Color = "Blue",
                Size = "Small",
                Dimensions = "10×10×20",
                ProductCategoryId = 5,
                CreatedAt = DateTime.UtcNow,
                UserCreatedId = string.Empty,
                IsAuction = true,
                AuctionStartTime = DateTime.UtcNow.AddHours(3),
                AuctionEndTime = DateTime.UtcNow.AddHours(48),
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "Product 6",
                Description = "Description for product 6",
                ImageUrl = "F1",
                Price = 75,
                OldPrice = null,
                InStock = true,
                Color = "Red",
                Size = "Large",
                Dimensions = "80×35×40",
                ProductCategoryId = 6,
                CreatedAt = DateTime.UtcNow,
                UserCreatedId = string.Empty,
                IsAuction = false,
                AuctionStartTime = null,
                AuctionEndTime = null,
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "Product 7",
                Description = "Description for product 7",
                ImageUrl = "A2",
                Price = 130,
                OldPrice = 160,
                InStock = true,
                Color = "Green",
                Size = "Large",
                Dimensions = "22×22×6",
                ProductCategoryId = 1,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                UserCreatedId = string.Empty,
                IsAuction = false,
                AuctionStartTime = null,
                AuctionEndTime = null,
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "Product 8",
                Description = "Description for product 8",
                ImageUrl = "B2",
                Price = 110,
                OldPrice = null,
                InStock = false,
                Color = "Blue",
                Size = "Medium",
                Dimensions = "28×28×38",
                ProductCategoryId = 2,
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                UserCreatedId = string.Empty,
                IsAuction = true,
                AuctionStartTime = DateTime.UtcNow.AddHours(4),
                AuctionEndTime = DateTime.UtcNow.AddHours(36),
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "Product 9",
                Description = "Description for product 9",
                ImageUrl = "E2",
                Price = 250,
                OldPrice = 300,
                InStock = true,
                Color = "Black",
                Size = "Large",
                Dimensions = "15×15×50",
                ProductCategoryId = 5,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UserCreatedId = string.Empty,
                IsAuction = false,
                AuctionStartTime = null,
                AuctionEndTime = null,
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "Product 10",
                Description = "Description for product 10",
                ImageUrl = "F2",
                Price = 400,
                OldPrice = null,
                InStock = true,
                Color = "Brown",
                Size = "Large",
                Dimensions = "40×20×70",
                ProductCategoryId = 6,
                CreatedAt = DateTime.UtcNow.AddDays(-4),
                UserCreatedId = string.Empty,
                IsAuction = true,
                AuctionStartTime = DateTime.UtcNow.AddHours(5),
                AuctionEndTime = DateTime.UtcNow.AddHours(72),
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "Product 11",
                Description = "Description for product 11",
                ImageUrl = "C2",
                Price = 200,
                OldPrice = 240,
                InStock = true,
                Color = "White",
                Size = "Medium",
                Dimensions = "45×45×32",
                ProductCategoryId = 3,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UserCreatedId = string.Empty,
                IsAuction = false,
                AuctionStartTime = null,
                AuctionEndTime = null,
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "Product 12",
                Description = "Description for product 12",
                ImageUrl = "D2",
                Price = 190,
                OldPrice = null,
                InStock = true,
                Color = "Red",
                Size = "Large",
                Dimensions = "65×35×40",
                ProductCategoryId = 4,
                CreatedAt = DateTime.UtcNow.AddDays(-6),
                UserCreatedId = string.Empty,
                IsAuction = true,
                AuctionStartTime = DateTime.UtcNow.AddHours(6),
                AuctionEndTime = DateTime.UtcNow.AddHours(48),
                IsAuctionClosed = false
            },
new Product
            {
                Name = "CloudSoft Pillow",
                Description = "Ultra-soft memory foam pillow for a restful sleep",
                ImageUrl = "A1",
                Price = 120,
                OldPrice = 150,
                InStock = true,
                Color = "White",
                Size = "Medium",
                Dimensions = "20×20×5",
                ProductCategoryId = 1,
                CreatedAt = DateTime.UtcNow,
                UserCreatedId = string.Empty,
                IsAuction = true,
                AuctionStartTime = DateTime.UtcNow.AddHours(1),
                AuctionEndTime = DateTime.UtcNow.AddHours(24),
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "ErgoComfort Chair",
                Description = "Ergonomic office chair with lumbar support",
                ImageUrl = "B1",
                Price = 95,
                OldPrice = null,
                InStock = true,
                Color = "Black",
                Size = "Large",
                Dimensions = "30×30×40",
                ProductCategoryId = 2,
                CreatedAt = DateTime.UtcNow,
                UserCreatedId = string.Empty,
                IsAuction = false,
                AuctionStartTime = null,
                AuctionEndTime = null,
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "Luna Circular Table",
                Description = "Modern circular dining table with a sleek finish",
                ImageUrl = "C1",
                Price = 220,
                OldPrice = 260,
                InStock = true,
                Color = "Brown",
                Size = "Medium",
                Dimensions = "40×40×30",
                ProductCategoryId = 3,
                CreatedAt = DateTime.UtcNow,
                UserCreatedId = string.Empty,
                IsAuction = true,
                AuctionStartTime = DateTime.UtcNow.AddHours(2),
                AuctionEndTime = DateTime.UtcNow.AddHours(30),
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "Oakwood Dining Table",
                Description = "Solid oak dining table for family gatherings",
                ImageUrl = "D1",
                Price = 180,
                OldPrice = null,
                InStock = true,
                Color = "Green",
                Size = "Large",
                Dimensions = "60×30×35",
                ProductCategoryId = 4,
                CreatedAt = DateTime.UtcNow,
                UserCreatedId = string.Empty,
                IsAuction = false,
                AuctionStartTime = null,
                AuctionEndTime = null,
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "Starlight Desk Lamp",
                Description = "Elegant LED desk lamp with adjustable brightness",
                ImageUrl = "E1",
                Price = 300,
                OldPrice = 350,
                InStock = true,
                Color = "Blue",
                Size = "Small",
                Dimensions = "10×10×20",
                ProductCategoryId = 5,
                CreatedAt = DateTime.UtcNow,
                UserCreatedId = string.Empty,
                IsAuction = true,
                AuctionStartTime = DateTime.UtcNow.AddHours(3),
                AuctionEndTime = DateTime.UtcNow.AddHours(48),
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "VelvetLux Sofa",
                Description = "Luxurious velvet sofa for modern living rooms",
                ImageUrl = "F1",
                Price = 75,
                OldPrice = null,
                InStock = true,
                Color = "Red",
                Size = "Large",
                Dimensions = "80×35×40",
                ProductCategoryId = 6,
                CreatedAt = DateTime.UtcNow,
                UserCreatedId = string.Empty,
                IsAuction = false,
                AuctionStartTime = null,
                AuctionEndTime = null,
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "DreamCloud Pillow",
                Description = "Cooling gel-infused pillow for ultimate comfort",
                ImageUrl = "A2",
                Price = 130,
                OldPrice = 160,
                InStock = true,
                Color = "Green",
                Size = "Large",
                Dimensions = "22×22×6",
                ProductCategoryId = 1,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                UserCreatedId = string.Empty,
                IsAuction = false,
                AuctionStartTime = null,
                AuctionEndTime = null,
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "PosturePro Chair",
                Description = "Adjustable chair for long working hours",
                ImageUrl = "B2",
                Price = 110,
                OldPrice = null,
                InStock = false,
                Color = "Blue",
                Size = "Medium",
                Dimensions = "28×28×38",
                ProductCategoryId = 2,
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                UserCreatedId = string.Empty,
                IsAuction = true,
                AuctionStartTime = DateTime.UtcNow.AddHours(4),
                AuctionEndTime = DateTime.UtcNow.AddHours(36),
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "MoonGlow Floor Lamp",
                Description = "Tall floor lamp with ambient lighting",
                ImageUrl = "E2",
                Price = 250,
                OldPrice = 300,
                InStock = true,
                Color = "Black",
                Size = "Large",
                Dimensions = "15×15×50",
                ProductCategoryId = 5,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UserCreatedId = string.Empty,
                IsAuction = false,
                AuctionStartTime = null,
                AuctionEndTime = null,
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "Rustic Bookshelf",
                Description = "Vintage-style bookshelf for home or office",
                ImageUrl = "F2",
                Price = 400,
                OldPrice = null,
                InStock = true,
                Color = "Brown",
                Size = "Large",
                Dimensions = "40×20×70",
                ProductCategoryId = 6,
                CreatedAt = DateTime.UtcNow.AddDays(-4),
                UserCreatedId = string.Empty,
                IsAuction = true,
                AuctionStartTime = DateTime.UtcNow.AddHours(5),
                AuctionEndTime = DateTime.UtcNow.AddHours(72),
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "Orbit Circular Table",
                Description = "Compact circular table for small spaces",
                ImageUrl = "C2",
                Price = 200,
                OldPrice = 240,
                InStock = true,
                Color = "White",
                Size = "Medium",
                Dimensions = "45×45×32",
                ProductCategoryId = 3,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UserCreatedId = string.Empty,
                IsAuction = false,
                AuctionStartTime = null,
                AuctionEndTime = null,
                IsAuctionClosed = false
            },
            new Product
            {
                Name = "Maple Conference Table",
                Description = "Large maple table for meetings",
                ImageUrl = "D2",
                Price = 190,
                OldPrice = null,
                InStock = true,
                Color = "Red",
                Size = "Large",
                Dimensions = "65×35×40",
                ProductCategoryId = 4,
                CreatedAt = DateTime.UtcNow.AddDays(-6),
                UserCreatedId = string.Empty,
                IsAuction = true,
                AuctionStartTime = DateTime.UtcNow.AddHours(6),
                AuctionEndTime = DateTime.UtcNow.AddHours(48),
                IsAuctionClosed = false
            }
                };

                if (Products?.Count > 0)
                {
                    foreach (var item in Products)
                    {
                        await _dbcontext.Products.AddAsync(item);
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }
            //if (!_dbcontext.CustomersReviews.Any())
            //{
            //    //// Reading the data form json file
            //    //var Data = File.ReadAllText("../Epic_Bid.Infrastructure.Persistence/_IdentityAndData/DataSeed/Reviews.json");
            //    //// Deserializing the data
            //    //var CustomersReviews = JsonSerializer.Deserialize<List<CustomerReview>>(Data);
            //    //// Adding the data to the database

            //    if (CustomersReviews?.Count > 0)
            //    {
            //        foreach (var item in CustomersReviews)
            //        {
            //            await _dbcontext.CustomersReviews.AddAsync(item);
            //        }
            //        await _dbcontext.SaveChangesAsync();
            //    }
            //}
            // Seeding DelvieryMethod
            if (!_dbcontext.DeliveryMethods.Any())
            {
                //var DeliveryMethodData = File.ReadAllText("../Epic_Bid.Infrastructure.Persistence/_IdentityAndData/DataSeed/delivery.json");
                //var deliveryMethod = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodData);
                var deliveryMethod = new List<DeliveryMethod>()
                {
                    new DeliveryMethod()
                    {
                         ShortName = "UPS1",
                         Description = "Fastest delivery time",
                         DeliveryTime = "1-2 Days",
                         Cost = 25,
                    },
                    new DeliveryMethod()
                    {
                         ShortName = "UPS2",
                         Description = "Standard delivery time",
                         DeliveryTime = "3-5 Days",
                         Cost = 15,
                    },
                    new DeliveryMethod()
                    {
                         ShortName = "UPS3",
                         Description = "Economy delivery time",
                         DeliveryTime = "5-7 Days",
                         Cost = 10,
                    },
                    new DeliveryMethod()
                    {
                         ShortName = "UPS4",
                         Description = "Free delivery",
                         DeliveryTime = "7-10 Days",
                         Cost = 0,
                    },
                    new DeliveryMethod()
                    {
                         ShortName = "UPS5",
                         Description = "Express delivery",
                         DeliveryTime = "1-3 Days",
                         Cost = 30,
                    },


                };
                    
                    if (deliveryMethod?.Count() > 0)
                    {
                        foreach (var item in deliveryMethod)
                        {
                            await _dbcontext.DeliveryMethods.AddAsync(item);
                        }
                        await _dbcontext.SaveChangesAsync();
                    }
                
                

            }

        }
    }
}
