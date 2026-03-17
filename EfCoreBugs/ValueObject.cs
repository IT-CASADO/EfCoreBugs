namespace Evo.Common;

#pragma warning disable S103 // Lines should not be too long
#pragma warning disable S4035 // the ValueObject class knows how to compare any derived classes (from the abstract GetEqualityComponents() method)
public abstract class ValueObject : IEquatable<ValueObject>
#pragma warning restore S4035 // the ValueObject class knows how to compare any derived classes (from the abstract GetEqualityComponents() method)
#pragma warning restore S103 // Lines should not be too long
{
    public bool Equals(ValueObject? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other is null)
        {
            return false;
        }

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is null)
        {
            return false;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        return GetEqualityComponents().SequenceEqual((obj as ValueObject)!.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 13;

            foreach (var obj in GetEqualityComponents())
            {
                hash = hash * 23 + (obj != null ? obj.GetHashCode() : 0);
            }

            return hash;
        }
    }

    public override string ToString() =>
        $"{GetType().Name}: ({string.Join(", ", GetEqualityComponents())})";

    protected abstract IEnumerable<object?> GetEqualityComponents();

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);
}
