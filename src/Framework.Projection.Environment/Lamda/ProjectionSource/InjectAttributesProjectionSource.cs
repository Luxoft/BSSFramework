using Framework.Core;

namespace Framework.Projection.Lambda;

internal class InjectAttributesProjectionSource : IProjectionSource
{
    private readonly IProjectionSource baseSource;

    private readonly ProjectionLambdaEnvironment environment;

    public InjectAttributesProjectionSource(IProjectionSource baseSource, ProjectionLambdaEnvironment environment)
    {
        this.baseSource = baseSource ?? throw new ArgumentNullException(nameof(baseSource));
        this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    public IEnumerable<IProjection> GetProjections()
    {
        var projections = this.baseSource.GetProjections().ToBuilders();

        foreach (var projectionBuilder in projections)
        {
            var initTypeAttributes = this.environment.GetProjectionTypeAttributeSource(projectionBuilder).GetAttributes().ToList();

            projectionBuilder.Attributes.Override(initTypeAttributes);

            foreach (var propertyBuilder in projectionBuilder.Properties)
            {
                var initPropAttributes = this.environment.GetProjectionPropertyAttributeSource(propertyBuilder).GetAttributes().ToList();

                propertyBuilder.Attributes.Override(initPropAttributes);
            }

            foreach (var customPropertyBuilder in projectionBuilder.CustomProperties)
            {
                var initCustomPropAttributes = this.environment.GetProjectionCustomPropertyAttributeSource(customPropertyBuilder).GetAttributes().ToList();

                customPropertyBuilder.Attributes.Override(initCustomPropAttributes);
            }
        }

        return projections;
    }
}
