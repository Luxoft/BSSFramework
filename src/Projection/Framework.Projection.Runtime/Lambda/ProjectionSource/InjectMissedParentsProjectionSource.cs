using CommonFramework;

using Framework.Core;
using Framework.Database.Mapping;
using Framework.Projection.Lambda._Extensions;
using Framework.Projection.Lambda.ProjectionBuilder;
using Framework.Relations;

namespace Framework.Projection.Lambda.ProjectionSource;

internal class InjectMissedParentsProjectionSource(ProjectionLambdaEnvironment environment, IProjectionSource baseSource) : IProjectionSource
{
    public IEnumerable<IProjection> GetProjections()
    {
        var builders = baseSource.GetProjections().ToBuilders();

        var missedParentRequest = from projection in builders

                                  from projectionProperty in projection.Properties.ToArray()

                                  where projectionProperty.Path.SingleMaybe()
                                                          .Select(prop => projectionProperty.IsCollection
                                                                          || environment.MetadataProxyProvider.Wrap(prop).HasAttribute<MappingAttribute>(attr => attr.IsOneToOne))
                                                          .GetValueOrDefault()

                                  let elementProjection =
                                      projectionProperty.ElementProjection.FromMaybe(() =>
                                                                                         $"ElementProjection property \"{projectionProperty.Name}\" for projection \"{projection.Name}\" not initialized")

                                  let potentialParentProperties = elementProjection.SourceType.GetProperties()
                                                                                   .Where(prop => prop.PropertyType.IsAssignableFrom(projection.SourceType)
                                                                                                  && prop.HasPrivateField()).ToList()

                                  let parentProperty = potentialParentProperties.Count == 1
                                                           ? potentialParentProperties.Single()
                                                           : potentialParentProperties.Single(
                                                               prop => environment.MetadataProxyProvider.Wrap(prop).HasAttribute<IsMasterAttribute>(),
                                                               () => new Exception(
                                                                   $"Parent property from \"{elementProjection.SourceType}\" to type \"{projection.SourceType}\" not found"))

                                  where !elementProjection.Properties.Select(prop => prop.Path).Contains(new PropertyPath([parentProperty]))

                                  select new { Projection = elementProjection, ParentProjection = projection, ParentProperty = parentProperty };


        foreach (var pair in missedParentRequest)
        {
            var newPropertyBuilder = new ProjectionPropertyBuilder(environment, pair.ParentProperty.ToGetLambdaExpression(pair.Projection.SourceType))
                                     {
                                         Role = ProjectionPropertyRole.MissedParent, ElementProjection = pair.ParentProjection, IgnoreSerialization = true
                                     };

            pair.Projection.Properties.Add(newPropertyBuilder);
        }

        return builders;
    }
}
