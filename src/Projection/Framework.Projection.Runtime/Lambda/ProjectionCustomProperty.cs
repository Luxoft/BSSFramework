using CommonFramework;

using Framework.Projection.Lambda._Extensions;

namespace Framework.Projection.Lambda;

/// <inheritdoc />
/// <typeparam name="TProperty">Тип свойства</typeparam>
public class ProjectionCustomProperty<TProperty> : IProjectionCustomProperty
{
    private readonly Lazy<TypeReferenceBase> lazyType;

    public ProjectionCustomProperty(string name, bool writable = false, Func<Projection<TProperty>>? getPropProjection = null, Type? collectionType = null, IEnumerable<string>? fetchs = null, IEnumerable<Attribute>? attributes = null)
    {
        if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("Value cannot be null or whitespace.", nameof(name)); }

        this.Name = name;
        this.Writable = writable;
        this.lazyType = LazyHelper.Create(() => getPropProjection == null ? (TypeReferenceBase)new TypeReferenceBase.FixedTypeReference(collectionType.SafeMakeProjectionCollectionType(typeof(TProperty)))
                                                          : new TypeReferenceBase.BuildTypeReference(typeof(TProperty), collectionType, false, getPropProjection()));

        this.Fetchs = fetchs.EmptyIfNull().ToList();
        this.Attributes = attributes.EmptyIfNull().ToList();
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public bool Writable { get; }

    /// <inheritdoc />
    public IReadOnlyList<string> Fetchs { get; }

    /// <inheritdoc />
    public IReadOnlyList<Attribute> Attributes { get; }

    /// <inheritdoc />
    public TypeReferenceBase Type => this.lazyType.Value;
}
