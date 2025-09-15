using Framework.Core;
using Framework.Persistent;

using CommonFramework;

namespace Framework.Projection.Lambda;

internal class AutoProjectionFactory : IFactory<ProjectionBuilder>
{
    private readonly ProjectionLambdaEnvironment environment;

    private readonly ProjectionBuilder baseProjection;

    public AutoProjectionFactory(ProjectionLambdaEnvironment environment, ProjectionBuilder baseProjection)
    {
        this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
        this.baseProjection = baseProjection ?? throw new ArgumentNullException(nameof(baseProjection));
    }

    public ProjectionBuilder Create()
    {
        var defaultProperties = this.baseProjection.Properties.Where(prop => prop.ElementProjection == null || prop.ElementProjection.Role == ProjectionRole.Default).ToList();

        var externalNodes = defaultProperties.Select(prop => new ProjectionPath(prop.Path.WithExpand(), new LastProjectionProperty(prop.Name, prop.ElementProjection))).ToNodes();

        return this.InternalCreate(this.baseProjection.SourceType, this.baseProjection.Name, externalNodes);
    }

    private ProjectionBuilder InternalCreate(Type domainType, string projectionName, IEnumerable<Node<ProjectionNodeValue>> nodes)
    {
        if (projectionName == null) throw new ArgumentNullException(nameof(projectionName));
        if (nodes == null) throw new ArgumentNullException(nameof(nodes));

        var properties = from node in nodes

                         let propertyPair = node.Value

                         let property = propertyPair.Property

                         where !this.environment.IsIdentityProperty(property)

                         from projectionProperty in this.InternalCreateProperties(domainType, projectionName, node)

                         select projectionProperty;

        return new ProjectionBuilder(domainType)
               {
                       Name = projectionName,
                       Role = ProjectionRole.AutoNode,
                       Properties = properties.ToList()
               };
    }

    private IEnumerable<ProjectionPropertyBuilder> InternalCreateProperties(Type domainType, string projectionName, Node<ProjectionNodeValue> node)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (projectionName == null) throw new ArgumentNullException(nameof(projectionName));
        if (node == null) throw new ArgumentNullException(nameof(node));

        var property = node.Value.Property;


        if (this.environment.IsPersistent(property.PropertyType) && node.Children.Any())
        {
            var elementProjection = this.InternalCreate(
                                                        property.PropertyType.GetCollectionElementTypeOrSelf(),
                                                        $"{projectionName}_AutoProp_{property.Name}",
                                                        node.Children);

            yield return new ProjectionPropertyBuilder(property.ToLambdaExpression(domainType), "_Auto")
                         {
                                 ElementProjection = elementProjection,
                                 Role = ProjectionPropertyRole.AutoNode,
                                 IgnoreSerialization = true
                         };
        }
        else
        {
            foreach (var projectionPath in node.ToPaths())
            {
                var lastPropertyValue = projectionPath.LastProperty;

                yield return new ProjectionPropertyBuilder(projectionPath.PropertyPath.ToLambdaExpression(domainType), $"_Last_{lastPropertyValue.PropertyName}")
                             {
                                     Role = ProjectionPropertyRole.LastAutoNode,
                                     IgnoreSerialization = true,
                                     ElementProjection = lastPropertyValue.ElementProjection
                             };
            }
        }
    }
}
