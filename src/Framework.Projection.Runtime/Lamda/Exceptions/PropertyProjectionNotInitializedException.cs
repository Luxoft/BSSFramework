namespace Framework.Projection.Lambda;

internal class PropertyProjectionNotInitializedException : Exception
{
    public PropertyProjectionNotInitializedException(Type elementType, string projectionName = null, string propertyName = null)
    {
        this.ElementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
        this.ProjectionName = projectionName;
        this.PropertyName = propertyName;
    }

    public override string Message =>
            this.ProjectionName == null || this.PropertyName == null
                    ? $"Pure persistent type \"{this.ElementType}\" for projection property not allowed. Parameter `getPropProjection` must be initialized."
                    : $"Pure persistent type \"{this.ElementType}\" for projection property \"{this.ProjectionName}.{this.PropertyName}\" not allowed. Parameter `getPropProjection` must be initialized.";

    public Type ElementType { get; }

    public string ProjectionName { get; }

    public string PropertyName { get; }
}
