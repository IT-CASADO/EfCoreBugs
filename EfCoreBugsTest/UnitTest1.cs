using EfCoreBugs;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EfCoreBugsTest
{
    public class UnitTest1
    {
        [Fact]
        public void SplitQueryWithHierarchyIdFilter()
        {
            // arrange
            MigrateDatabase();

            var context = new MyContext();
            var myBlog = new Blog() { Name = "MyBlog" };

            myBlog.Posts.Add(new Post() { Title = "T1", Blog = myBlog });
            myBlog.Posts.Add(new Post() { Title = "T2", Blog = myBlog });

            context.Blogs.Add(myBlog);

            context.SaveChanges();

            // act

            var action = () =>
            {
                HierarchyId[] nodeIdFilter = [HierarchyId.GetRoot()];
                context
                    .Blogs
                    .Include(b => b.Posts)
                    .Where(b => nodeIdFilter.Contains(b.NodeId))
                    .AsSplitQuery()
                    .ToList();
            };

            // assert
            action.Should().NotThrow();
        }

        private void MigrateDatabase()
        {
            var context = new MyContext();

            if (context.Database.GetDbConnection().Database != "")
            {
                context.Database.Migrate();
            }
        }
    }
}
