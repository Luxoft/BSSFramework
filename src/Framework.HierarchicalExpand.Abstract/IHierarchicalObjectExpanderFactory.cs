using System;

using JetBrains.Annotations;

namespace Framework.HierarchicalExpand
{
    public interface IHierarchicalObjectExpanderFactory<TIdent>
    {
        IHierarchicalObjectExpander<TIdent> Create([NotNull] Type domainType);
    }
}
