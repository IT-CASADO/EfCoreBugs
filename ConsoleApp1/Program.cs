using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

await using var ctx = new BlogContext();
await ctx.Database.EnsureDeletedAsync();
await ctx.Database.EnsureCreatedAsync();

await ctx.Database.ExecuteSqlRawAsync(
    @"

DECLARE @MyValue1 DECIMAL(38,10) = 99998888889688880000.00000000
DECLARE @MyValue2 DECIMAL(38,10) = 39998519999997200103600.0000000000

INSERT INTO [Blogs]([MyNumber])
   VALUES(@MyValue1)

INSERT INTO [Blogs]([MyNumber])
   VALUES(@MyValue2)
  
"
);

foreach (var blog in ctx.Blogs)
{
    Console.WriteLine($"BlogId: {blog.Id}, MyValue: {blog.MyNumber}");
}

ctx.Blogs.Add(new Blog());
await ctx.SaveChangesAsync();

public class BlogContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder
            .UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=EfCoreBugs;Trusted_Connection=True;ConnectRetryCount=0"
            )
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging();
}

public class Blog
{
    public int Id { get; set; }

    [Precision(38, 10)]
    public decimal MyNumber { get; set; } = 99998888889688880000.00000000m;
}
