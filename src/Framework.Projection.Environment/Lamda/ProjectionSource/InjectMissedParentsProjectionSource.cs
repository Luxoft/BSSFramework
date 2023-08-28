using Framework.Core;
using Framework.Persistent;
using Framework.Persistent.Mapping;

namespace Framework.Projection.Lambda;

internal class InjectMissedParentsProjectionSource : IProjectionSource
{
    private readonly IProjectionSource baseSource;


    public InjectMissedParentsProjectionSource(IProjectionSource baseSource)
    {
        this.baseSource = baseSource ?? throw new ArgumentNullException(nameof(baseSource));
    }


    public IEnumerable<IProjection> GetProjections()
    {
        var builders = this.baseSource.GetProjections().ToBuilders();

        var missedParentRequest = from projection in builders

                                  from projectionProperty in projection.Properties.ToArray()

                                  where projectionProperty.Path.SingleMaybe().Select(prop => projectionProperty.IsCollection || prop.HasAttribute<MappingAttribute>(attr => attr.IsOneToOne)).GetValueOrDefault()

                                  let elementProjection = projectionProperty.ElementProjection.FromMaybe(() => $"ElementProjection property \"{projectionProperty.Name}\" for projection \"{projection.Name}\" not initialized")

                                  let potentialParentProperties = elementProjection.SourceType.GetProperties().Where(prop => prop.PropertyType.IsAssignableFrom(projection.SourceType) && prop.HasPrivateField()).ToList()

                                  let parentProperty = potentialParentProperties.Count == 1 ? potentialParentProperties.Single() : potentialParentProperties.Single(prop => prop.HasAttribute<IsMasterAttribute>(), () => new Exception($"Parent property from \"{elementProjection.SourceType}\" to type \"{projection.SourceType}\" not found"))

                                  where !elementProjection.Properties.Select(prop => prop.Path).Contains(new PropertyPath(new[] { parentProperty }))

                                  select new
                                         {
                                                 Projection = elementProjection,

                                                 ParentProjection = projection,

                                                 ParentProperty = parentProperty
                                         };


        foreach (var pair in missedParentRequest)
        {
            var newPropertyBuilder = new ProjectionPropertyBuilder(pair.ParentProperty.ToLambdaExpression(pair.Projection.SourceType))
                                     {
                                             Role = ProjectionPropertyRole.MissedParent,
                                             ElementProjection = pair.ParentProjection,
                                             IgnoreSerialization = true
                                     };

            pair.Projection.Properties.Add(newPropertyBuilder);
        }

        return builders;
    }
}
