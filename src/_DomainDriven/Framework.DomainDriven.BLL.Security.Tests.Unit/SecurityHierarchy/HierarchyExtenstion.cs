using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Persistent;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy
{
    public static class HierarchyExtenstion
    {
        public static IEnumerable<TAncestorLink> ToAncestorLinks<TDomainObject, TAncestorLink, TDomainToAncestorOrChildLink>(this IEnumerable<TDomainObject> source)
            where TDomainObject : IHierarchicalPersistentDomainObjectBase<TDomainObject, Guid>
            where TAncestorLink : IModifiedHierarchicalAncestorLink<TDomainObject, TDomainToAncestorOrChildLink, Guid>, new() where TDomainToAncestorOrChildLink : IHierarchicalToAncestorOrChildLink<TDomainObject, Guid>
        {
            var thisToParent = source.ToDictionary(z => z.Id, z => z.Parent);

            foreach (var domainObject in source)
            {
                var currentValue = domainObject;
                while(true)
                {
                    TDomainObject parent;
                    if (thisToParent.TryGetValue(currentValue.Id, out parent) && null != parent)
                    {
                        yield return new TAncestorLink() { Ancestor = parent, Child = domainObject };
                        currentValue = parent;
                    }
                    else
                    {
                        break;
                    }
                }
                yield return new TAncestorLink() { Ancestor = domainObject, Child = domainObject };
            }
        }
    }
}