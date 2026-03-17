// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Evo.Common;

#pragma warning disable S103 // Lines should not be too long
#pragma warning disable S1210 // "IComparable" is only implemented to fix EF Core batch update behavior. IdentityObject is not meant to be compared with comparison operators
// Note: I would also like to apply the microsoft converter here with attribute based configuration.
// But it is not possible because of an issue (or different behavior) in the System.Text.Json library.
// see: https://github.com/dotnet/runtime/issues/47892
// it seems that even the factory way is not working as expected.
//[System.Text.Json.Serialization.JsonConverter(typeof(IdentityObjectJsonConverterFactory))]
public abstract class IdentityObject<TValue>(TValue value)
    : ValueObject,
        IComparable,
        IComparable<IdentityObject<TValue>>
#pragma warning restore S1210
#pragma warning restore S103 // Lines should not be too long
    where TValue : IComparable
{
    public TValue Value { get; private init; } = value;

    public int CompareTo(object? obj) => CompareTo(obj as IdentityObject<TValue>);

    public int CompareTo(IdentityObject<TValue>? other)
    {
        if (other == null)
        {
            return -1;
        }

        return Value.CompareTo(other.Value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString()!;

    public static implicit operator TValue(IdentityObject<TValue> identityObject) =>
        identityObject.Value;

    public static bool operator >(IdentityObject<TValue> a, IdentityObject<TValue> b) =>
        a.Value.CompareTo(b.Value) > 0;

    public static bool operator >=(IdentityObject<TValue> a, IdentityObject<TValue> b) =>
        a.Value.CompareTo(b.Value) >= 0;

    public static bool operator <(IdentityObject<TValue> a, IdentityObject<TValue> b) =>
        a.Value.CompareTo(b.Value) < 0;

    public static bool operator <=(IdentityObject<TValue> a, IdentityObject<TValue> b) =>
        a.Value.CompareTo(b.Value) <= 0;
}
