using JetBrains.Annotations;

namespace Framework.HierarchicalExpand;

public interface IHierarchicalObjectExpanderFactory<TIdent>
{
    IHierarchicalObjectExpander<TIdent> Create([NotNull] Type domainType);

    IHierarchicalObjectQueryableExpander<TIdent> CreateQuery([NotNull] Type domainType);
}
