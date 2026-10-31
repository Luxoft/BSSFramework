using System.Linq.Expressions;

using Anch.Core;

using Framework.Core;
using Framework.Projection.Lambda.Extensions;

namespace Framework.Projection.Lambda;

/// <summary>
/// Type reference
/// </summary>
public abstract class TypeReferenceBase
{
    /// <summary>
    /// Attempt to replace a projection
    /// </summary>
    /// <param name="mapFunc">Functor</param>
    /// <returns></returns>
    public abstract TypeReferenceBase TryOverrideElementProjection(Func<IProjection, IProjection> mapFunc);

    /// <summary>
    /// Fixed type
    /// </summary>
    public class FixedTypeReference(Type propertyType) : TypeReferenceBase
    {
        /// <summary>
        /// Property type
        /// </summary>
        public Type PropertyType { get; } = propertyType ?? throw new ArgumentNullException(nameof(propertyType));

        public override string ToString() => this.PropertyType.ToString();

        public override TypeReferenceBase TryOverrideElementProjection(Func<IProjection, IProjection> mapFunc)
        {
            if (mapFunc is null) { throw new ArgumentNullException(nameof(mapFunc)); }

            return this;
        }
    }

    /// <summary>
    /// Dynamic type
    /// </summary>
    public class BuildTypeReference : TypeReferenceBase
    {
        public BuildTypeReference(Type elementType, Type? collectionType, bool isNullable, IProjection? elementProjection)
        {
            this.ElementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
            this.CollectionType = collectionType;
            this.IsNullable = isNullable;
            this.ElementProjection = elementProjection;
        }

        public BuildTypeReference(LambdaExpression expression)
        {
            if (expression is null) { throw new ArgumentNullException(nameof(expression)); }

            this.ElementType = expression.ReturnType.GetNullableElementType() ?? expression.ReturnType.GetCollectionElementTypeOrSelf();
            this.CollectionType = expression.ReturnType.GetProjectionCollectionType();
            this.IsNullable = expression.ReturnType.IsValueType && expression.ToPropertyPath().HasReferenceResult();
        }

        /// <summary>
        /// Property element type
        /// </summary>
        public Type ElementType { get; }

        /// <summary>
        /// Collection type
        /// </summary>
        public Type? CollectionType { get; }

        /// <summary>
        /// The property is a collection
        /// </summary>
        public bool IsCollection => this.CollectionType is not null;

        /// <summary>
        /// The property has a Nullable type
        /// </summary>
        public bool IsNullable { get; }

        /// <summary>
        /// Projection onto the property type
        /// </summary>
        public IProjection? ElementProjection { get; }

        public override string ToString() => $"ElementType = {this.ElementType}, IsCollection = {this.IsCollection} | IsNullable = {this.IsNullable} | ElementProjection = {this.ElementProjection}";

        public override TypeReferenceBase TryOverrideElementProjection(Func<IProjection, IProjection> mapFunc)
        {
            if (mapFunc is null) { throw new ArgumentNullException(nameof(mapFunc)); }

            return this.ElementProjection is null ? this : new BuildTypeReference(this.ElementType, this.CollectionType, this.IsNullable, mapFunc(this.ElementProjection));
        }
    }
}
