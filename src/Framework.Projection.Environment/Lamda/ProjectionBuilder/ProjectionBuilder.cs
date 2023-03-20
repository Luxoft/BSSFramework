using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda;

internal class ProjectionBuilder : IProjection
{
    public ProjectionBuilder([NotNull] IProjection sourceProjection)
    {
        if (sourceProjection == null) throw new ArgumentNullException(nameof(sourceProjection));

        this.SourceType = sourceProjection.SourceType;
        this.Name = sourceProjection.Name;
        this.BLLView = sourceProjection.BLLView;
        this.Role = sourceProjection.Role;
        this.Attributes = sourceProjection.Attributes.ToList();
        this.FilterAttributes = sourceProjection.FilterAttributes.ToList();
    }

    public ProjectionBuilder([NotNull] Type sourceType)
    {
        this.SourceType = sourceType ?? throw new ArgumentNullException(nameof(sourceType));
    }


    public Type SourceType { get; }


    public string Name { get; set; }

    public bool BLLView { get; set; }

    public ProjectionRole Role { get; set; }


    public List<ProjectionPropertyBuilder> Properties { get; set; } = new List<ProjectionPropertyBuilder>();

    public List<ProjectionCustomPropertyBuilder> CustomProperties { get; } = new List<ProjectionCustomPropertyBuilder>();

    public List<Attribute> Attributes { get; set; } = new List<Attribute>();

    public List<ProjectionFilterAttribute> FilterAttributes { get; set; } = new List<ProjectionFilterAttribute>();

    public bool IgnoreIdSerialization { get; set; }

    IReadOnlyList<IProjectionProperty> IProjection.Properties => this.Properties;

    IReadOnlyList<Attribute> IProjectionAttributeProvider.Attributes => this.Attributes;

    IReadOnlyList<ProjectionFilterAttribute> IProjection.FilterAttributes => this.FilterAttributes;

    IReadOnlyList<IProjectionCustomProperty> IProjection.CustomProperties => this.CustomProperties;

    public override string ToString()
    {
        return this.Name;
    }
}
