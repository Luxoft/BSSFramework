using System.Reflection;

using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven;

public class ExpandFetchPathFactory : DTOFetchPathFactory
{
    public ExpandFetchPathFactory(Type persistentDomainObjectBase, int maxRecurseLevel = 1)
            : base(persistentDomainObjectBase, maxRecurseLevel)
    {

    }


    protected override PropertyLoadNode ExpandNode(PropertyLoadNode node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));

        var withoutIgnoreNode = node.WhereP(property => !property.HasAttribute<IgnoreFetchAttribute>(), false);

        var pureNode = withoutIgnoreNode.WhereP(property => !property.HasAttribute<ExpandPathAttribute>() && !property.HasAttribute<FetchPathAttribute>(), false);

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
        if (property == null) throw new ArgumentNullException(nameof(property));

        var expandPath = property.GetExpandPath();

        if (expandPath is null)
        {
            foreach (var fetchPath in property.GetFetchPaths())
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
                                            new Dictionary<PropertyInfo, PropertyLoadNode>
                                            {
                                                    { property, this.ToLoadNode(nestedType, propertyPath.Tail) }
                                            },
                                            new PropertyInfo[0]);
            }
            else
            {
                return new PropertyLoadNode(
                                            domainType,
                                            new Dictionary<PropertyInfo, PropertyLoadNode>(),
                                            new[] { property });
            }
        }
        else
        {
            return new PropertyLoadNode(
                                        domainType,
                                        new Dictionary<PropertyInfo, PropertyLoadNode>(),
                                        new PropertyInfo[0]);
        }
    }
}
