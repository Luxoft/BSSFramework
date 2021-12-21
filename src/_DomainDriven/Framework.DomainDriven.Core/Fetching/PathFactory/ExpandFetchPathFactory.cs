using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.DomainDriven
{
    public class ExpandFetchPathFactory : DTOFetchPathFactory, IFetchPathFactory<FetchBuildRule.DTOFetchBuildRule>
    {
        public ExpandFetchPathFactory(Type persistentDomainObjectBase, int maxRecurseLevel = 1)
            : base(persistentDomainObjectBase, maxRecurseLevel)
        {

        }


        protected override PropertyLoadNode ExpandNode([NotNull] PropertyLoadNode node)
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

        private IEnumerable<PropertyPath> ExpandProperty([NotNull] PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var expandPath = property.GetExpandPath();

            if (expandPath == null)
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

        private PropertyLoadNode ToLoadNode(Type domainType, [NotNull] PropertyPath propertyPath)
        {
            if (propertyPath == null) throw new ArgumentNullException(nameof(propertyPath));

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


        IEnumerable<PropertyPath> IFactory<Type, FetchBuildRule.DTOFetchBuildRule, IEnumerable<PropertyPath>>.Create(Type type, FetchBuildRule.DTOFetchBuildRule rule)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (rule == null) throw new ArgumentNullException(nameof(rule));

            return this.Create(type, rule.DTOType);
        }
    }
}
