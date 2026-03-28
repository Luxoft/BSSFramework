using Framework.Projection.Lambda._Extensions;
using Framework.Projection.Lambda.ProjectionSource._Base;
using Framework.Projection.Lambda.ProjectionSource.AutoProjection;

namespace Framework.Projection.Lambda.ProjectionSource;

internal class CreateAutoNodesProjectionSource(IProjectionSource baseSource, ProjectionLambdaEnvironment environment) : IProjectionSource
{
    private readonly IProjectionSource baseSource = baseSource ?? throw new ArgumentNullException(nameof(baseSource));

    private readonly ProjectionLambdaEnvironment environment = environment ?? throw new ArgumentNullException(nameof(environment));

    public IEnumerable<IProjection> GetProjections()
    {
        var builders = this.baseSource.GetProjections().ToBuilders();

        foreach (var projectionBuilder in builders)
        {
            if (projectionBuilder.Role == ProjectionRole.Default)
            {
                var rootAutoProjection = new AutoProjectionFactory(this.environment, projectionBuilder).Create();

                var addProperties = rootAutoProjection.Properties.Where(prop => prop.Role == ProjectionPropertyRole.AutoNode).ToList();

                projectionBuilder.Properties.AddRange(addProperties);
            }
        }

        return builders.GetAllProjections();
    }
}
