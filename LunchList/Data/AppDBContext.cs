using LunchList.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LunchList.DTO;

namespace LunchList.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<GroceryList> GroceryLists { get; set; }  // Make sure this is present

        public DbSet<GroceryItem> GroceryItems { get; set; }
        public DbSet<GroceryListItem> GroceryListItems { get; set; }

        public DbSet<Retailer> Retailers { get; set; }
        public DbSet<RetailerProduct> RetailersProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroceryItemDTO>().HasNoKey();

            
            modelBuilder.Entity<GroceryList>().ToTable("grocery_lists");
            modelBuilder.Entity<GroceryListItem>().ToTable("grocery_list_items");
            modelBuilder.Entity<Retailer>().ToTable("retailers");
            modelBuilder.Entity<RetailerProduct>().ToTable("retailer_products");


            // Foreign key: GroceryItem → Retailers_products
            modelBuilder.Entity<GroceryItem>()
                .HasOne(gi => gi.RetailerProduct)
                .WithMany()
                .HasForeignKey(gi => gi.Retailer_Product_Id);
           

            // Foreign key: Retailers_products → Retailers
            modelBuilder.Entity<RetailerProduct>()
                .HasOne(rp => rp.Retailer)
                .WithMany(r => r.RetailerProducts)
                .HasForeignKey(rp => rp.RetailerId);
            
            
            modelBuilder.Entity<GroceryListItem>(entity =>
            {
                entity.ToTable("grocery_list_items");

                entity.HasKey(e => new { e.GroceryListId, e.GroceryItemId });

                entity.Property(e => e.GroceryListId).HasColumnName("grocery_list_id");
                entity.Property(e => e.GroceryItemId).HasColumnName("grocery_item_id");

                entity.HasOne(e => e.GroceryItem)
                    .WithMany(i => i.GroceryListItems)
                    .HasForeignKey(e => e.GroceryItemId)
                    .HasPrincipalKey(i => i.Id);

                entity.HasOne(e => e.GroceryList)
                    .WithMany(l => l.GroceryListItems)
                    .HasForeignKey(e => e.GroceryListId)
                    .HasPrincipalKey(l => l.Id);
            });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .LogTo(Console.WriteLine, LogLevel.Information);
        }
        
    }

}