using Framework.Core;

// ReSharper disable once CheckNamespace
namespace Framework.Validation;

/// <summary>
/// Validation context
/// </summary>
public interface IValidationContext : IValidationContextBase, IServiceProviderContainer;

/// <summary>
/// Validation context
/// </summary>
/// <typeparam name="TSource">Object type</typeparam>
public interface IValidationContext<out TSource> : IValidationContextBase<TSource>, IValidationContext;

/// <summary>
/// Validation context
/// </summary>
/// <typeparam name="TSource">Object type</typeparam>
/// <typeparam name="TValidationMap">Metadata</typeparam>
public interface IValidationContext<out TSource, out TValidationMap> : IValidationContext<TSource>, IValidationContextBase<TSource, TValidationMap>
        where TValidationMap : class;
