using Evo.Common;

namespace Evo.BusinessFramework;

//file sealed class ItemIdTypeConverter : IdentityObjectTypeConverter<ItemId, Guid>;

//[TypeConverter(typeof(ItemIdTypeConverter))]
public class ItemId(Guid value) : GuidIdentity(value)
{
    /// <summary>
    /// Constructor is used by AutoMapper by default configuration
    /// </summary>
    /// <param name="external"></param>
    public static readonly ItemId Empty = new(Guid.Empty);

    public static ItemId Next() => new(Guid.NewGuid());
}
