using System.Reflection;

using Framework.Validation.Validators;

namespace Framework.Validation.Map;

/// <summary>
/// Metadata of a validated property
/// </summary>
public interface IPropertyValidationMap
{
    /// <summary>
    /// Property name
    /// </summary>
    string PropertyName { get; }

    /// <summary>
    /// Property
    /// </summary>
    PropertyInfo Property { get; }

    /// <summary>
    /// The property is a collection
    /// </summary>
    bool IsCollection { get; }

    /// <summary>
    /// The property is a detail
    /// </summary>
    bool IsDetail { get; }

    /// <summary>
    /// The property is composite
    /// </summary>
    bool IsExpanded { get; }

    /// <summary>
    /// Type metadata
    /// </summary>
    IClassValidationMap ReflectedTypeMap { get; }

    /// <summary>
    /// Property type metadata
    /// </summary>
    IClassValidationMap PropertyTypeMap { get; }
}

/// <summary>
/// Metadata of a validated property
/// </summary>
/// <typeparam name="TSource">Validated type</typeparam>
public interface IPropertyValidationMap<in TSource> : IPropertyValidationMap
{
    /// <summary>
    /// Metadata of the type to which the property belongs
    /// </summary>
    new IClassValidationMap<TSource> ReflectedTypeMap { get; }
}

/// <summary>
/// Metadata of a validated property
/// </summary>
/// <typeparam name="TSource">Validated type</typeparam>
/// <typeparam name="TProperty">Property type</typeparam>
public interface IPropertyValidationMap<in TSource, in TProperty> : IPropertyValidationMap<TSource>, IValidatorCollection<IPropertyValidator<TSource, TProperty>>;

/// <summary>
/// Metadata of a validated single property
/// </summary>
/// <typeparam name="TSource">Validated type</typeparam>
/// <typeparam name="TProperty">Property type</typeparam>
public interface ISinglePropertyValidationMap<in TSource, in TProperty> : IPropertyValidationMap<TSource, TProperty>
{
    /// <summary>
    /// Property type metadata
    /// </summary>
    new IClassValidationMap<TProperty> PropertyTypeMap { get; }
}

/// <summary>
/// Metadata of a validated collection property
/// </summary>
/// <typeparam name="TSource">Validated type</typeparam>
/// <typeparam name="TProperty">Property type</typeparam>
/// <typeparam name="TElement">Collection element type</typeparam>
public interface ICollectionPropertyValidationMap<in TSource, in TProperty, in TElement> : IPropertyValidationMap<TSource, TProperty>
        where TProperty : IEnumerable<TElement>
{
    /// <summary>
    /// Property type metadata
    /// </summary>
    new IClassValidationMap<TElement> PropertyTypeMap { get; }
}
