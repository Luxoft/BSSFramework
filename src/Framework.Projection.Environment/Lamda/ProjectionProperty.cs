using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

using Framework.Core;

using Framework.Persistent;
using Framework.Persistent.Mapping;

namespace Framework.Projection.Lambda;

/// <summary>
/// Проекционное свойство
/// </summary>
public abstract class ProjectionProperty<TExpression, TElement> : IProjectionProperty
        where TExpression : LambdaExpression
{
    private readonly Lazy<Projection<TElement>> lazyElementProjection;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="path">Путь до свойства</param>
    /// <param name="name">Имя свойства</param>
    /// <param name="getPropProjection">Тип проекции свойства</param>
    /// <param name="ignoreSerialization">Игноририрование сериализации</param>
    /// <param name="attributes">Дополнительные атрибуты свойства</param>
    protected ProjectionProperty(TExpression path, string name, Func<Projection<TElement>> getPropProjection, bool ignoreSerialization, IEnumerable<Attribute> attributes)
    {
        this.Expression = path ?? throw new ArgumentNullException(nameof(path));
        this.IgnoreSerialization = ignoreSerialization;
        this.Path = this.Expression.ToPropertyPath().WithExpand();
        this.Name = name ?? path.ToPath().Replace(".", "");
        this.IsNullable = typeof(TElement).IsValueType && this.Path.HasReferenceResult();

        this.ElementType = typeof(TElement).GetNullableElementTypeOrSelf();

        this.lazyElementProjection = getPropProjection.Maybe(v => LazyHelper.Create(v));

        this.Path.Where(prop => !prop.IsPersistent()).Foreach(prop => throw new Exception($"Projection property \"{prop.Name}\" of path \"{this.Expression}\" must be persistent"));
        this.Attributes = (attributes ?? throw new ArgumentNullException(nameof(attributes))).ToReadOnlyCollection();
    }

    /// <inheritdoc />
    public ProjectionPropertyRole Role { get; } = ProjectionPropertyRole.Default;

    /// <summary>
    /// Expression-путь до свойства
    /// </summary>
    public TExpression Expression { get; }

    /// <inheritdoc />
    public PropertyPath Path { get; }

    /// <inheritdoc />
    public string Name { get; }

    public IProjection ElementProjection => this.lazyElementProjection.Maybe(v => v.Value);

    /// <inheritdoc />
    public abstract Type SourceType { get; }

    /// <summary>
    /// Проекция на тип свойства
    /// </summary>
    public Type ElementType { get; }

    /// <summary>
    /// Тип коллекции
    /// </summary>
    public abstract Type CollectionType { get; }

    /// <summary>
    /// Свойство является коллекцией
    /// </summary>
    public bool IsCollection => this.CollectionType != null;

    /// <summary>
    /// Свойство имеет Nullable-тип
    /// </summary>
    public bool IsNullable { get; }

    /// <inheritdoc />
    public IReadOnlyList<Attribute> Attributes { get; }

    /// <inheritdoc />
    public bool IgnoreSerialization { get; }

    LambdaExpression IProjectionProperty.Expression => this.Expression;

    TypeReferenceBase.BuildTypeReference IProjectionProperty.Type => new TypeReferenceBase.BuildTypeReference(this.ElementType, this.CollectionType, this.IsNullable, this.ElementProjection);

    PropertyInfo IProjectionProperty.VirtualExplicitInterfaceProperty { get; } = null;
}
