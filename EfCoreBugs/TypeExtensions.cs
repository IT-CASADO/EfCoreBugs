using System.Reflection;

namespace Evo.Common;

public static class TypeExtensions
{
	public static bool IsNumericType(this Type type) => IsFloatingPointType(type) || IsIntegerType(type);

	public static bool IsIntegerType(this Type type)
	{
		var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
		return Type.GetTypeCode(underlyingType) switch
		{
			TypeCode.Byte => true,
			TypeCode.SByte => true,
			TypeCode.UInt16 => true,
			TypeCode.UInt32 => true,
			TypeCode.UInt64 => true,
			TypeCode.Int16 => true,
			TypeCode.Int32 => true,
			TypeCode.Int64 => true,
			_ => false,
		};
	}

	public static bool IsFloatingPointType(this Type type)
	{
		var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
		return Type.GetTypeCode(underlyingType) switch
		{
			TypeCode.Decimal => true,
			TypeCode.Double => true,
			TypeCode.Single => true,
			_ => false,
		};
	}

	public static bool IsDateType(this Type type)
	{
		var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
		return Type.GetTypeCode(underlyingType) switch
		{
			TypeCode.DateTime => true,
			_ => false,
		};
	}

	public static bool IsDateOnlyType(this Type type)
	{
		var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
		return underlyingType == typeof(DateOnly);
	}

	public static bool IsBooleanType(this Type type)
	{
		var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
		return Type.GetTypeCode(underlyingType) switch
		{
			TypeCode.Boolean => true,
			_ => false,
		};
	}

	public static bool IsStringType(this Type type)
	{
		var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
		return Type.GetTypeCode(underlyingType) switch
		{
			TypeCode.String => true,
			_ => false,
		};
	}

	public static bool IsGuidType(this Type type)
	{
		var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

		return underlyingType == typeof(Guid);
	}

	/// <summary>
	/// Checks if the type is a subclass of <see cref="IdentityObject{TValue}"/>
	/// </summary>
	/// <param name="type"></param>
	/// <c>true</c> if the specified <see cref="System.Type" /> a subclass of <see cref="IdentityObject{TValue}"/>
	/// otherwise, <c>false</c>.
	public static bool IsIdentityType(this Type type) => type.IsSubclassOfRawGeneric(typeof(IdentityObject<>));

	public static bool IsIdentityType<TValue>(this Type type) =>
		IsIdentityType(type) && type.GetUnderlyingIdentityType() == typeof(TValue);

	public static bool IsSubclassOfRawGeneric(this Type? toCheck, Type generic)
	{
		while (toCheck != null && toCheck != typeof(object))
		{
			var cur = toCheck.GetTypeInfo().IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;

			if (generic == cur)
			{
				return true;
			}

			toCheck = toCheck.GetTypeInfo().BaseType;
		}

		return false;
	}

	/// <summary>
	/// Returns the identity value type for types that inherit from <see cref="IdentityObject{TValue}"/>.
	/// Otherwise, returns the type itself.
	/// </summary>
	/// <param name="type"></param>
	public static Type GetUnderlyingIdentityType(this Type type)
	{
		if (!type.IsIdentityType())
		{
			return type;
		}

		var identityValueType = type;
		while (identityValueType.BaseType != typeof(ValueObject))
		{
			if (identityValueType.BaseType is null)
			{
				return type;
			}

			identityValueType = identityValueType.BaseType;
		}

		return identityValueType.GetGenericArguments()[0];
	}

	public static MethodInfo? GetStaticMethod(this Type type, string name) =>
		type.GetMethod(name, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);

	public static string GetFriendlyName(this Type type)
	{
		if (!type.IsGenericType)
		{
			return type.Name;
		}

		var genericTypeDefinition = type.GetGenericTypeDefinition();
		var genericArguments = type.GetGenericArguments();
		return $"{genericTypeDefinition.Name}<{string.Join(", ", genericArguments.Select(x => x.Name))}>";
	}

	#region from AwesomeAssertions
	public static bool IsAssignableToOpenGeneric(this Type type, Type definition)
	{
		// The CLR type system does not consider anything to be assignable to an open generic type.
		// For the purposes of test assertions, the user probably means that the subject type is
		// assignable to any generic type based on the given generic type definition.
		if (definition.IsInterface)
		{
			return type.IsImplementationOfOpenGeneric(definition);
		}

		return type == definition || type.IsDerivedFromOpenGeneric(definition);
	}

	private static bool IsImplementationOfOpenGeneric(this Type type, Type definition)
	{
		// check subject against definition
		if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == definition)
		{
			return true;
		}

		// check subject's interfaces against definition
		return type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == definition);
	}

	public static bool IsDerivedFromOpenGeneric(this Type type, Type definition)
	{
		if (type == definition)
		{
			// do not consider a type to be derived from itself
			return false;
		}

		// check subject and its base types against definition
		for (var baseType = type; baseType is not null; baseType = baseType.BaseType)
		{
			if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == definition)
			{
				return true;
			}
		}

		return false;
	}
	#endregion from AwesomeAssertions

	public static string ToSimpleTypeString(this Type type)
	{
		if (type.IsGenericType)
		{
			var args = type.GetGenericArguments();

			if (args.Length == 1 && type.IsAssignableTo(typeof(System.Collections.IEnumerable)))
			{
				return $"ARRAY of {args[0].Name}";
			}
		}

		return type.Name;
	}
}
