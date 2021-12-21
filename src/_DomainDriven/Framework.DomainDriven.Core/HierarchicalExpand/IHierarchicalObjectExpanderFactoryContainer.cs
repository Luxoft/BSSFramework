using Framework.HierarchicalExpand;

namespace Framework.DomainDriven
{
    public interface IHierarchicalObjectExpanderFactoryContainer<TIdent>
    {
        IHierarchicalObjectExpanderFactory<TIdent> HierarchicalObjectExpanderFactory { get; }
    }
}
