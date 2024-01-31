using Microsoft.EntityFrameworkCore;

namespace EfCoreBugs
{
    public class Blog
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<Post> Posts { get; } = new List<Post>();

        public HierarchyId NodeId { get; set; } = HierarchyId.GetRoot();
    }

    public class Post
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string Content { get; set; } = "Content";
        public DateTime PublishedOn { get; set; } = DateTime.UtcNow;
        public bool Archived { get; set; }

        public int BlogId { get; set; }
        public required Blog Blog { get; set; }
    }
}
