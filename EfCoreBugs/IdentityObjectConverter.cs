using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Evo.Common.Data.EntityFramework;

public class IdentityObjectConverter<TModel, TProvider>(ConverterMappingHints? mappingHints = null)
    : ValueConverter<TModel, TProvider>(
        id => id.Value,
        v => (TModel)Activator.CreateInstance(typeof(TModel), v)!,
        mappingHints
    )
    where TModel : IdentityObject<TProvider>
    where TProvider : IComparable;
