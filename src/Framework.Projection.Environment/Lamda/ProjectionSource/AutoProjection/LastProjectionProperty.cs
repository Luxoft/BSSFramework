using System;

namespace Framework.Projection.Lambda
{
    internal class LastProjectionProperty
    {
        public LastProjectionProperty(string propertyName, ProjectionBuilder elementProjection)
        {
            this.PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            this.ElementProjection = elementProjection;
        }

        public string PropertyName { get; }

        public ProjectionBuilder ElementProjection { get; }
    }
}
