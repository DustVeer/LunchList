using LunchList.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LunchList.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<Grocery_list_items> GroceryListItems { get; set; }
        public DbSet<Grocery_lists> GroceryLists { get; set; }  // Make sure this is present
        public DbSet<Grocery_items> GroceryItems { get; set; }
        public DbSet<Retailers> Retailers { get; set; }
        public DbSet<Retailers_products> RetailersProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grocery_items>().ToTable("grocery_items");
            modelBuilder.Entity<Grocery_list_items>().ToTable("grocery_list_items");
            modelBuilder.Entity<Grocery_lists>().ToTable("grocery_lists");
            modelBuilder.Entity<Retailers>().ToTable("retailers");
            modelBuilder.Entity<Retailers_products>().ToTable("retailers_products");

            // Composite key on Grocery_list_items
            modelBuilder.Entity<Grocery_list_items>()
                .HasKey(gli => new { gli.grocery_list_id, gli.grocery_item_id });

            // Foreign key: Grocery_list_items → Grocery_lists
            modelBuilder.Entity<Grocery_list_items>()
                .HasOne(gli => gli.GroceryList)
                .WithMany(gl => gl.GroceryListItems)
                .HasForeignKey(gli => gli.grocery_list_id);

            // Foreign key: Grocery_list_items → Grocery_items
            modelBuilder.Entity<Grocery_list_items>()
                .HasOne(gli => gli.GroceryItem)
                .WithMany()
                .HasForeignKey(gli => gli.grocery_item_id);

            // Foreign key: Grocery_items → Retailers_products
            modelBuilder.Entity<Grocery_items>()
                .HasOne(gi => gi.RetailerProduct)
                .WithMany()
                .HasForeignKey(gi => gi.retailer_product_id);

            // Foreign key: Retailers_products → Retailers
            modelBuilder.Entity<Retailers_products>()
                .HasOne(rp => rp.Retailer)
                .WithMany(r => r.RetailerProducts)
                .HasForeignKey(rp => rp.retailer_id);
        }
    }

}