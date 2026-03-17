//using System.ComponentModel;
//using System.Diagnostics.CodeAnalysis;
//using System.Globalization;

//namespace Evo.Common;

//public abstract class IdentityObjectTypeConverter<T, TValue> : TypeConverter
//    where T : IdentityObject<TValue>
//    where TValue : IComparable
//{
//    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
//        sourceType == typeof(string)
//        || sourceType == typeof(Guid)
//        || base.CanConvertFrom(context, sourceType);

//    public override bool CanConvertTo(
//        ITypeDescriptorContext? context,
//        [NotNullWhen(true)] Type? destinationType
//    ) =>
//        destinationType == typeof(string)
//        || destinationType == typeof(Guid)
//        || base.CanConvertTo(context, destinationType);

//    public override object? ConvertFrom(
//        ITypeDescriptorContext? context,
//        CultureInfo? culture,
//        object value
//    ) =>
//        value switch
//        {
//            Guid guidValue when typeof(TValue) == typeof(Guid) => Activator.CreateInstance(
//                typeof(T),
//                guidValue
//            ),
//            string stringValue when typeof(TValue) == typeof(Guid) => Activator.CreateInstance(
//                typeof(T),
//                Guid.Parse(stringValue)
//            ),
//            string stringValue when typeof(TValue) == typeof(string) => Activator.CreateInstance(
//                typeof(T),
//                stringValue
//            ),
//            _ => base.ConvertFrom(context, culture, value),
//        };

//    public override object? ConvertTo(
//        ITypeDescriptorContext? context,
//        CultureInfo? culture,
//        object? value,
//        Type destinationType
//    ) =>
//        value switch
//        {
//            GuidIdentity guidIdentity when destinationType == typeof(Guid) => guidIdentity.Value,
//            GuidIdentity guidIdentity when destinationType == typeof(string) =>
//                guidIdentity.Value.ToString(),
//            StringIdentity stringIdentity when destinationType == typeof(string) =>
//                stringIdentity.Value,
//            _ => base.ConvertTo(context, culture, value, destinationType),
//        };
//}
