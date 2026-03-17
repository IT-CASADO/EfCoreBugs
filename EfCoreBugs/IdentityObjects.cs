namespace Evo.Common;

public abstract class GuidIdentity(Guid value) : IdentityObject<Guid>(value), IConvertible
{
    public TypeCode GetTypeCode() => throw new NotSupportedException();

    public bool ToBoolean(IFormatProvider? provider) => throw new NotSupportedException();

    public byte ToByte(IFormatProvider? provider) => throw new NotSupportedException();

    public char ToChar(IFormatProvider? provider) => throw new NotSupportedException();

    public DateTime ToDateTime(IFormatProvider? provider) => throw new NotSupportedException();

    public decimal ToDecimal(IFormatProvider? provider) => throw new NotSupportedException();

    public double ToDouble(IFormatProvider? provider) => throw new NotSupportedException();

    public short ToInt16(IFormatProvider? provider) => throw new NotSupportedException();

    public int ToInt32(IFormatProvider? provider) => throw new NotSupportedException();

    public long ToInt64(IFormatProvider? provider) => throw new NotSupportedException();

    public sbyte ToSByte(IFormatProvider? provider) => throw new NotSupportedException();

    public float ToSingle(IFormatProvider? provider) => throw new NotSupportedException();

    public string ToString(IFormatProvider? provider) => Value.ToString();

    public object ToType(Type conversionType, IFormatProvider? provider)
    {
        if (conversionType == typeof(Guid))
        {
            return Value;
        }

        throw new NotSupportedException();
    }

    public ushort ToUInt16(IFormatProvider? provider) => throw new NotSupportedException();

    public uint ToUInt32(IFormatProvider? provider) => throw new NotSupportedException();

    public ulong ToUInt64(IFormatProvider? provider) => throw new NotSupportedException();

    public static implicit operator Guid(GuidIdentity id) => id.Value;

    public static implicit operator Guid?(GuidIdentity? id) => id?.Value;
}
