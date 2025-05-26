using LunchList.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LunchList.DTO;

namespace LunchList.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<GroceryList> GroceryLists { get; set; }  // Make sure this is present
        public DbSet<Retailer> Retailers { get; set; }
        public DbSet<RetailerProduct> RetailersProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroceryItemDTO>().HasNoKey();

            modelBuilder.Entity<GroceryList>().ToTable("grocery_lists");
            modelBuilder.Entity<Retailer>().ToTable("retailers");
            modelBuilder.Entity<RetailerProduct>().ToTable("retailer_products");


            // Foreign key: Retailers_products → Retailers
            modelBuilder.Entity<RetailerProduct>()
                .HasOne(rp => rp.Retailer)
                .WithMany(r => r.RetailerProducts)
                .HasForeignKey(rp => rp.RetailerId);
        }
    }

}