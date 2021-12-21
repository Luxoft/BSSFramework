using System;
using System.Reflection;

namespace Framework.Projection.Lambda
{
    internal class ProjectionNodeValue
    {
        public ProjectionNodeValue(PropertyInfo property)
        {
            this.Property = property ?? throw new ArgumentNullException(nameof(property));
        }

        public PropertyInfo Property { get; }
    }
}
