using Anch.Core;

using Framework.Core;
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
                var newPath = environment.PropertyPathService.WithExpand(propertyBuilder.Path);

                if (newPath != propertyBuilder.Path)
                {
                    propertyBuilder.Path = newPath;
                    propertyBuilder.IsNullable = propertyBuilder.Expression.ReturnType.IsValueType && propertyBuilder.Path.HasReferenceResult();
                }

                propertyBuilder.Path.Where(prop => !prop.IsPersistent()).Foreach(prop => throw new Exception($"Projection property \"{prop.Name}\" of path \"{propertyBuilder.Path}\" must be persistent"));
            }
        }

        return builders.GetAllProjections();
    }
}

