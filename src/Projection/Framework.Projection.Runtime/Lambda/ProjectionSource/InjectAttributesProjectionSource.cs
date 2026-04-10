using Framework.Core;
using Framework.Projection.Lambda._Extensions;
using Framework.Projection.Lambda.ProjectionSource._Base;

namespace Framework.Projection.Lambda.ProjectionSource;

internal class InjectAttributesProjectionSource(ProjectionLambdaEnvironment environment, IProjectionSource baseSource) : IProjectionSource
{
    private readonly IProjectionSource baseSource = baseSource ?? throw new ArgumentNullException(nameof(baseSource));

    private readonly ProjectionLambdaEnvironment environment = environment ?? throw new ArgumentNullException(nameof(environment));

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
