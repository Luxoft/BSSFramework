using CommonFramework;

using Framework.Database.Mapping.Extensions;
using Framework.Projection.Lambda.Extensions;

namespace Framework.Projection.Lambda.ProjectionSource;

internal class ExpandPathProjectionSource(ProjectionLambdaEnvironment environment, IProjectionSource baseSource) : IProjectionSource
{
    public IEnumerable<IProjection> GetProjections()
    {
        var builders = baseSource.GetProjections().ToBuilders();

        foreach (var projectionBuilder in builders)
        {
            foreach (var propertyBuilder in projectionBuilder.Properties)
            {
                propertyBuilder.Path = environment.PropertyPathService.WithExpand(propertyBuilder.Path);

                propertyBuilder.Path.Where(prop => !prop.IsPersistent()).Foreach(prop => throw new Exception($"Projection property \"{prop.Name}\" of path \"{propertyBuilder.Path}\" must be persistent"));
            }
        }

        return builders.GetAllProjections();
    }
}
