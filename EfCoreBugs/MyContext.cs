using Evo.Common.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EfCoreBugs
{
    public class MyContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var connection =
                @"Server=(localdb)\mssqllocaldb;Database=EfCoreBugs;Trusted_Connection=True;ConnectRetryCount=0";

            optionsBuilder.UseSqlServer(
                connection,
                options =>
                {
                    options.CommandTimeout(300);
                }
            );

            optionsBuilder.ReplaceService<IValueConverterSelector, EvoValueConverterSelector>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().HasKey(e => e.Id);
        }
    }
}
