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

        public DbSet<GroceryItem> GroceryItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroceryItem>().ToTable("grocery_lists");
        }
    }
}