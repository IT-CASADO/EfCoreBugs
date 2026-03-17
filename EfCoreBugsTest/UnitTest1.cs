using EfCoreBugs;
using Evo.BusinessFramework;
using FluentAssertions;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EfCoreBugsTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task LinqToDB_BulkCopy_Error()
        {
            // arrange
            MigrateDatabase();

            var context = new MyContext();
            await context.Items.ExecuteDeleteAsync();

            var item1 = new Item() { Id = ItemId.Next(), Name = "Item 1" };
            var children = Enumerable
                .Range(2, 1)
                .Select(i => new Item()
                {
                    Id = ItemId.Next(),
                    Name = "Item " + i,
                    ParentId = item1.Id,
                });

            // act


            var action = async () =>
            {
                await using var connection = context.CreateLinqToDBConnection();
                await connection.BulkCopyAsync(
                    new BulkCopyOptions(CheckConstraints: true),
                    [item1, .. children]
                );
            };

            // assert
            await action.Should().NotThrowAsync();
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
