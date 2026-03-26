using System.Linq.Expressions;

using CommonFramework;

using Framework.Core;

namespace Framework.Projection.Lambda;

/// <summary>
/// Ссылка на тип
/// </summary>
public abstract class TypeReferenceBase
{
    protected TypeReferenceBase()
    {
    }

    /// <summary>
    /// Попытка заменить проекцию
    /// </summary>
    /// <param name="mapFunc">Функтор</param>
    /// <returns></returns>
    public abstract TypeReferenceBase TryOverrideElementProjection(Func<IProjection, IProjection> mapFunc);

    /// <summary>
    /// Фиксированный тип
    /// </summary>
    public class FixedTypeReference : TypeReferenceBase
    {
        public FixedTypeReference(Type propertyType)
        {
            this.PropertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
        }

        /// <summary>
        /// Тип свойства
        /// </summary>
        public Type PropertyType { get; }

        public override string ToString()
        {
            return this.PropertyType.ToString();
        }

        public override TypeReferenceBase TryOverrideElementProjection(Func<IProjection, IProjection> mapFunc)
        {
            if (mapFunc == null) { throw new ArgumentNullException(nameof(mapFunc)); }

            return this;
        }
    }

    /// <summary>
    /// Динамический тип
    /// </summary>
    public class BuildTypeReference : TypeReferenceBase
    {
        public BuildTypeReference(Type elementType, Type collectionType, bool isNullable, IProjection elementProjection)
        {
            this.ElementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
            this.CollectionType = collectionType;
            this.IsNullable = isNullable;
            this.ElementProjection = elementProjection;
        }

        public BuildTypeReference(LambdaExpression expression)
        {
            if (expression == null) { throw new ArgumentNullException(nameof(expression)); }

            this.CollectionType = expression.ReturnType.GetProjectionCollectionType();

            this.IsNullable = expression.ReturnType.IsValueType && expression.ToPropertyPath().HasReferenceResult();
            this.ElementType = expression.ReturnType.GetNullableElementType() ?? expression.ReturnType.GetCollectionElementTypeOrSelf();
        }

        /// <summary>
        /// Тип элемента свойства
        /// </summary>
        public Type ElementType { get; }

        /// <summary>
        /// Тип коллекции
        /// </summary>
        public Type CollectionType { get; }

        /// <summary>
        /// Свойство является коллекцией
        /// </summary>
        public bool IsCollection => this.CollectionType != null;

        /// <summary>
        /// Свойство имеет Nullable-тип
        /// </summary>
        public bool IsNullable { get; }

        /// <summary>
        /// Проекция на тип свойства
        /// </summary>
        public IProjection ElementProjection { get; }

        public override string ToString()
        {
            return $"ElementType = {this.ElementType}, IsCollection = {this.IsCollection} | IsNullable = {this.IsNullable} | ElementProjection = {this.ElementProjection}";
        }

        public override TypeReferenceBase TryOverrideElementProjection(Func<IProjection, IProjection> mapFunc)
        {
            if (mapFunc == null) { throw new ArgumentNullException(nameof(mapFunc)); }

            return this.ElementProjection == null ? this : new BuildTypeReference(this.ElementType, this.CollectionType, this.IsNullable, mapFunc(this.ElementProjection));
        }
    }
}
