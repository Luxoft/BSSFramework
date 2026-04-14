using Framework.Projection.Lambda.Extensions;
using Framework.Projection.Lambda.ProjectionSource.AutoProjection;

namespace Framework.Projection.Lambda.ProjectionSource;

internal class CreateAutoNodesProjectionSource(ProjectionLambdaEnvironment environment, IProjectionSource baseSource) : IProjectionSource
{
    public IEnumerable<IProjection> GetProjections()
    {
        var builders = baseSource.GetProjections().ToBuilders();

        foreach (var projectionBuilder in builders)
        {
            if (projectionBuilder.Role == ProjectionRole.Default)
            {
                var rootAutoProjection = new AutoProjectionFactory(environment, projectionBuilder).Create();

                var addProperties = rootAutoProjection.Properties.Where(prop => prop.Role == ProjectionPropertyRole.AutoNode).ToList();

                projectionBuilder.Properties.AddRange(addProperties);
            }
        }

        return builders.GetAllProjections();
    }
}
