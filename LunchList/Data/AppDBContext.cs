using LunchList.Models;
using Microsoft.EntityFrameworkCore;
using LunchList.DTO;

namespace LunchList.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<GroceryItem> GroceryItems { get; set; }
        public DbSet<GroceryListItem> GroceryListItems { get; set; }
        public DbSet<GroceryList> GroceryLists { get; set; }
        public DbSet<Retailer> Retailers { get; set; }
        public DbSet<RetailerProduct> RetailersProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroceryItem>(entity =>
            {
                entity.ToTable("grocery_items");

                entity.HasOne(gi => gi.RetailerProduct)
                      .WithMany()
                      .HasForeignKey(gi => gi.Retailer_Product_Id);
            });

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

            modelBuilder.Entity<GroceryList>().ToTable("grocery_lists");
            modelBuilder.Entity<Retailer>().ToTable("retailers");
            modelBuilder.Entity<RetailerProduct>().ToTable("retailer_products");

            modelBuilder.Entity<GroceryItemDTO>().HasNoKey();
            modelBuilder.Entity<GroceryItem>().Ignore("GroceryListId");
            modelBuilder.Entity<GroceryItem>().Ignore("GroceryListId1");
            modelBuilder.Entity<GroceryItem>().Ignore("GroceryListId2");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}
