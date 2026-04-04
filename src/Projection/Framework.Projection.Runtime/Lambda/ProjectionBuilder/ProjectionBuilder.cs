namespace Framework.Projection.Lambda.ProjectionBuilder;

internal class ProjectionBuilder : IProjection
{
    public ProjectionBuilder(IProjection sourceProjection)
    {
        if (sourceProjection == null) throw new ArgumentNullException(nameof(sourceProjection));

        this.SourceType = sourceProjection.SourceType;
        this.Name = sourceProjection.Name;
        this.BLLView = sourceProjection.BLLView;
        this.Role = sourceProjection.Role;
        this.Attributes = sourceProjection.Attributes.ToList();
        this.FilterAttributes = sourceProjection.FilterAttributes.ToList();
    }

    public ProjectionBuilder(Type sourceType) => this.SourceType = sourceType ?? throw new ArgumentNullException(nameof(sourceType));

    public Type SourceType { get; }


    public string Name { get; set; }

    public bool BLLView { get; set; }

    public ProjectionRole Role { get; set; }


    public List<ProjectionPropertyBuilder> Properties { get; set; } = [];

    public List<ProjectionCustomPropertyBuilder> CustomProperties { get; } = [];

    public List<Attribute> Attributes { get; set; } = [];

    public List<ProjectionFilterAttribute> FilterAttributes { get; set; } = [];

    public bool IgnoreIdSerialization { get; set; }

    IReadOnlyList<IProjectionProperty> IProjection.Properties => this.Properties;

    IReadOnlyList<Attribute> IProjectionAttributeProvider.Attributes => this.Attributes;

    IReadOnlyList<ProjectionFilterAttribute> IProjection.FilterAttributes => this.FilterAttributes;

    IReadOnlyList<IProjectionCustomProperty> IProjection.CustomProperties => this.CustomProperties;

    public override string ToString() => this.Name;
}
