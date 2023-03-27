using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda;

internal class ProjectionPropertyBuilder : IProjectionProperty
{
    public ProjectionPropertyBuilder([NotNull] IProjectionProperty projectionProperty)
    {
        if (projectionProperty == null) throw new ArgumentNullException(nameof(projectionProperty));

        this.SourceType = projectionProperty.SourceType;
        this.Expression = projectionProperty.Expression;
        this.Path = projectionProperty.Path;
        this.CollectionType = projectionProperty.Type.CollectionType;
        this.IsNullable = projectionProperty.Type.IsNullable;
        this.ElementType = projectionProperty.Type.ElementType;
        this.Name = projectionProperty.Name;
        this.Role = projectionProperty.Role;
        this.IgnoreSerialization = projectionProperty.IgnoreSerialization;
        this.Attributes = projectionProperty.Attributes.ToList();
        this.VirtualExplicitInterfaceProperty = projectionProperty.VirtualExplicitInterfaceProperty;
    }

    public ProjectionPropertyBuilder([NotNull] LambdaExpression path, string namePostfix = null)
    {
        this.Expression = path ?? throw new ArgumentNullException(nameof(path));
        this.SourceType = path.Parameters.Single().Type;
        this.Path = path.ToPropertyPath().WithExpand();
        this.CollectionType = this.Expression.ReturnType.GetProjectionCollectionType();
        this.IsNullable = this.Expression.ReturnType.IsValueType && this.Path.HasReferenceResult();
        this.ElementType = this.Expression.ReturnType.GetNullableElementType() ?? this.Expression.ReturnType.GetCollectionElementTypeOrSelf();
        this.Name = this.Expression.ToPath().Replace(".", string.Empty) + namePostfix;
        this.Attributes = new List<Attribute>();
    }


    public Type SourceType { get; }

    public LambdaExpression Expression { get; }

    public PropertyPath Path { get; }

    public Type CollectionType { get; }

    public bool IsCollection => this.CollectionType != null;

    public bool IsNullable { get; }

    public Type ElementType { get; }


    public string Name { get; set; }

    public ProjectionPropertyRole Role { get; set; }

    public ProjectionBuilder ElementProjection { get; set; }

    public bool IgnoreSerialization { get; set; }

    public PropertyInfo VirtualExplicitInterfaceProperty { get; set; }

    public List<Attribute> Attributes { get; set; }

    IReadOnlyList<Attribute> IProjectionAttributeProvider.Attributes => this.Attributes;

    TypeReferenceBase.BuildTypeReference IProjectionProperty.Type => new TypeReferenceBase.BuildTypeReference(this.ElementType, this.CollectionType, this.IsNullable, this.ElementProjection);


    public override string ToString()
    {
        return this.Name;
    }
}
