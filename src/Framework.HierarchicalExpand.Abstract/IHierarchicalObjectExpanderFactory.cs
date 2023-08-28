namespace Framework.HierarchicalExpand;

public interface IHierarchicalObjectExpanderFactory<TIdent>
{
    IHierarchicalObjectExpander<TIdent> Create(Type domainType);

    IHierarchicalObjectQueryableExpander<TIdent> CreateQuery(Type domainType);
}
