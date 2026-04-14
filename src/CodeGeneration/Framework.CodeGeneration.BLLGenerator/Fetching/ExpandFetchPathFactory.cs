using System.Reflection;
using Framework.BLL.Domain.Fetching;
using Framework.BLL.Domain.Persistent.Attributes;
using Framework.BLL.Extensions;
using Framework.BLL.Services;
using Framework.Core;
using Framework.ExtendedMetadata;

namespace Framework.CodeGeneration.BLLGenerator.Fetching;

public class ExpandFetchPathFactory(
    IMetadataProxyProvider metadataProxyProvider,
    IPropertyPathService propertyPathService, Type persistentDomainObjectBase, int maxRecurseLevel = 1) :
    DTOFetchPathFactory(metadataProxyProvider, persistentDomainObjectBase, maxRecurseLevel)
{
    protected override PropertyLoadNode ExpandNode(PropertyLoadNode node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));

        var withoutIgnoreNode = node.WhereP(property => !metadataProxyProvider.Wrap(property).HasAttribute<IgnoreFetchAttribute>(), false);

        var pureNode = withoutIgnoreNode.WhereP(
            property =>
            {
                var wrapProp = metadataProxyProvider.Wrap(property);

                return !wrapProp.HasAttribute<ExpandPathAttribute>() && !wrapProp.HasAttribute<FetchPathAttribute>();
            },
            false);

        var pathProperties = withoutIgnoreNode.Properties.Keys.Except(pureNode.Properties.Keys)
                                              .Concat(withoutIgnoreNode.PrimitiveProperties.Except(pureNode.PrimitiveProperties))
                                              .ToList();

        var newNodes = pathProperties.SelectMany(this.ExpandProperty)
                                     .Select(property => this.ToLoadNode(node.DomainType, property));

        var preResult = newNodes.Aggregate(pureNode, (state, addNode) => state + addNode);

        return preResult.SelectN(this.ExpandNode, false);
    }

    private IEnumerable<PropertyPath> ExpandProperty(PropertyInfo property)
    {
        var expandPath = propertyPathService.TryGetExpandPath(property);

        if (expandPath is null)
        {
            foreach (var fetchPath in propertyPathService.GetPropertyPaths<FetchPathAttribute>(property))
            {
                yield return fetchPath;
            }
        }
        else
        {
            yield return expandPath;
        }
    }

    private PropertyLoadNode ToLoadNode(Type domainType, PropertyPath propertyPath)
    {
        if (propertyPath.Any())
        {
            var property = propertyPath.Head;

            var nestedType = property.GetNestedType();

            var isTransferType = this.IsTransferType(nestedType);

            if (isTransferType)
            {
                return new PropertyLoadNode(
                    domainType,
                    new Dictionary<PropertyInfo, PropertyLoadNode> { { property, this.ToLoadNode(nestedType, propertyPath.Tail) } },
                    []);
            }
            else
            {
                return new PropertyLoadNode(
                    domainType,
                    new Dictionary<PropertyInfo, PropertyLoadNode>(),
                    [property]);
            }
        }
        else
        {
            return new PropertyLoadNode(
                domainType,
                new Dictionary<PropertyInfo, PropertyLoadNode>(),
                []);
        }
    }
}
