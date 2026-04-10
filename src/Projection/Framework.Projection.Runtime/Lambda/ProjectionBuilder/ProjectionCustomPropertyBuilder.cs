namespace Framework.Projection.Lambda.ProjectionBuilder;

internal class ProjectionCustomPropertyBuilder : IProjectionCustomProperty
{
    public ProjectionCustomPropertyBuilder(IProjectionCustomProperty customProperty)
    {
        if (customProperty == null) { throw new ArgumentNullException(nameof(customProperty)); }

        this.Name = customProperty.Name;
        this.Writable = customProperty.Writable;
        this.Fetchs = customProperty.Fetchs;
        this.Attributes = customProperty.Attributes.ToList();
    }

    public ProjectionCustomPropertyBuilder()
    {
    }


    public string Name { get; set; }

    public bool Writable { get; set; }

    public TypeReferenceBase Type { get; set; }

    public IReadOnlyList<string> Fetchs { get; set; } = [];

    public List<Attribute> Attributes { get; set; } = [];


    IReadOnlyList<Attribute> IProjectionAttributeProvider.Attributes => this.Attributes;
}
