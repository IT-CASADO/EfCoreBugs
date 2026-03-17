using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Concurrent;

namespace Evo.Common.Data.EntityFramework;

public class EvoValueConverterSelector(ValueConverterSelectorDependencies dependencies)
    : ValueConverterSelector(dependencies)
{
    private readonly ConcurrentDictionary<
        (Type ModelClrType, Type ProviderClrType),
        ValueConverterInfo
    > _converters = new();

    public override IEnumerable<ValueConverterInfo> Select(
        Type modelClrType,
        Type? providerClrType = null
    )
    {
        var baseConverters = base.Select(modelClrType, providerClrType);
        foreach (var converter in baseConverters)
        {
            yield return converter;
        }

        var underlyingModelType = UnwrapNullableType(modelClrType);

        if (
            underlyingModelType is not null
            && underlyingModelType.IsSubclassOfRawGeneric(typeof(IdentityObject<>))
        )
        {
            var underlyingProviderType = underlyingModelType.GetUnderlyingIdentityType();

            var valueConverterInfo = _converters.GetOrAdd(
                (underlyingModelType, underlyingProviderType),
                (x) =>
                {
                    var converterType = typeof(IdentityObjectConverter<,>).MakeGenericType(
                        x.ModelClrType,
                        x.ProviderClrType
                    );

                    return new(
                        modelClrType: x.ModelClrType,
                        providerClrType: x.ProviderClrType,
                        factory: info =>
                            (ValueConverter)
                                Activator.CreateInstance(converterType, info.MappingHints)!
                    );
                }
            );

            yield return valueConverterInfo;

            if (underlyingProviderType == typeof(Guid))
            {
                var valueConverterInfo2 = _converters.GetOrAdd(
                    (underlyingModelType, typeof(string)),
                    (x) =>
                    {
                        var converterType = typeof(IdentityObjectConverter<,>).MakeGenericType(
                            x.ModelClrType,
                            typeof(Guid)
                        );

                        return new(
                            modelClrType: x.ModelClrType,
                            providerClrType: x.ProviderClrType,
                            factory: info =>
                                (ValueConverter)
                                    Activator.CreateInstance(converterType, info.MappingHints)!
                        );
                    }
                );

                yield return valueConverterInfo2;
            }
        }
    }

    private static Type? UnwrapNullableType(Type? type)
    {
        if (type is null)
        {
            return null;
        }

        return Nullable.GetUnderlyingType(type) ?? type;
    }
}
