using Microsoft.EntityFrameworkCore;


namespace CoronaStore.Models
{
    public class CoronaPageContext : DbContext
    {
        public CoronaPageContext()
        {
        }

        public CoronaPageContext(DbContextOptions<CoronaPageContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename=./PageDB.db");
            }
        }
    }
}