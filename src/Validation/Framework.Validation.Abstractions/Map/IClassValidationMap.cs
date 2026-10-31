using Framework.Validation.Validators;

namespace Framework.Validation.Map;

/// <summary>
/// Metadata of a validated type
/// </summary>
public interface IClassValidationMap
{
    /// <summary>
    /// Validated type name
    /// </summary>
    string TypeName { get; }

    /// <summary>
    /// Validated type
    /// </summary>
    Type Type { get; }

    /// <summary>
    /// List of validated properties
    /// </summary>
    IReadOnlyCollection<IPropertyValidationMap> PropertyMaps { get; }
}

/// <summary>
/// Metadata of a validated type
/// </summary>
/// <typeparam name="TSource">Validated type</typeparam>
public interface IClassValidationMap<in TSource> : IClassValidationMap, IValidatorCollection<IClassValidator<TSource>>
{
    /// <summary>
    /// List of validated properties
    /// </summary>
    new IReadOnlyCollection<IPropertyValidationMap<TSource>> PropertyMaps { get; }
}
