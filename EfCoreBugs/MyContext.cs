using Microsoft.EntityFrameworkCore;

namespace EfCoreBugs
{
    public class MyContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

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
                    options.UseHierarchyId();
                }
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfiguration(new SomeEntityConfiguration());
        }
    }

    //public class SomeEntityConfiguration : IEntityTypeConfiguration<SomeEntity>
    //{
    //    public void Configure(EntityTypeBuilder<SomeEntity> builder)
    //    {
    //        builder.HasKey(i => i.Id);

    //        builder.Property(i => i.Name).IsRequired();

    //        builder.OwnsOne<SomeField>(
    //            "_fieldA",
    //            b =>
    //            {
    //                b.Property("PropA").HasColumnName("FieldAPropA").IsRequired(true);

    //                b.Property("PropB").HasColumnName("FieldAPropB").IsRequired(true);
    //            }
    //        );

    //        builder.Navigation("_fieldA").IsRequired();

    //        builder.ToTable("SomeEntity");
    //    }
    //}
}
