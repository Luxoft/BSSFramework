namespace Framework.Projection.Lambda.Exceptions;

internal class PropertyProjectionNotInitializedException(Type elementType, string projectionName = null, string propertyName = null) : Exception
{
    public override string Message =>
        this.ProjectionName == null || this.PropertyName == null
            ? $"Pure persistent type \"{this.ElementType}\" for projection property not allowed. Parameter `getPropProjection` must be initialized."
            : $"Pure persistent type \"{this.ElementType}\" for projection property \"{this.ProjectionName}.{this.PropertyName}\" not allowed. Parameter `getPropProjection` must be initialized.";

    public Type ElementType { get; } = elementType ?? throw new ArgumentNullException(nameof(elementType));

    public string ProjectionName { get; } = projectionName;

    public string PropertyName { get; } = propertyName;
}
