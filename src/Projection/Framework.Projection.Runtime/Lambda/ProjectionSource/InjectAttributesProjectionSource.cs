using Framework.Core;
using Framework.Projection.Lambda._Extensions;

namespace Framework.Projection.Lambda.ProjectionSource;

internal class InjectAttributesProjectionSource(ProjectionLambdaEnvironment environment, IProjectionSource baseSource) : IProjectionSource
{
    public IEnumerable<IProjection> GetProjections()
    {
        var projections = baseSource.GetProjections().ToBuilders();

        foreach (var projectionBuilder in projections)
        {
            var initTypeAttributes = environment.GetProjectionTypeAttributeSource(projectionBuilder).GetAttributes().ToList();

            projectionBuilder.Attributes.Override(initTypeAttributes);

            foreach (var propertyBuilder in projectionBuilder.Properties)
            {
                var initPropAttributes = environment.GetProjectionPropertyAttributeSource(propertyBuilder).GetAttributes().ToList();

                propertyBuilder.Attributes.Override(initPropAttributes);
            }

            foreach (var customPropertyBuilder in projectionBuilder.CustomProperties)
            {
                var initCustomPropAttributes = environment.GetProjectionCustomPropertyAttributeSource(customPropertyBuilder).GetAttributes().ToList();

                customPropertyBuilder.Attributes.Override(initCustomPropAttributes);
            }
        }

        return projections;
    }
}
