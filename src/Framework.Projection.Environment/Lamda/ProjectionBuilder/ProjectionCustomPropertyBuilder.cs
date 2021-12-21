using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda
{
    internal class ProjectionCustomPropertyBuilder : IProjectionCustomProperty
    {
        public ProjectionCustomPropertyBuilder([NotNull] IProjectionCustomProperty customProperty)
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

        public IReadOnlyList<string> Fetchs { get; set; } = new string[0];

        public List<Attribute> Attributes { get; set; } = new List<Attribute>();


        IReadOnlyList<Attribute> IProjectionAttributeProvider.Attributes => this.Attributes;
    }
}
