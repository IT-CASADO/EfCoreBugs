using Evo.BusinessFramework;

namespace EfCoreBugs
{
    public class Item
    {
        public required ItemId Id { get; set; }
        public ItemId? ParentId { get; set; }
        public required string Name { get; set; }
    }
}
