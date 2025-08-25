using SecuritySystem.HierarchicalExpand;

namespace Framework.DomainDriven;

public interface IHierarchicalObjectExpanderFactoryContainer
{
    IHierarchicalObjectExpanderFactory HierarchicalObjectExpanderFactory { get; }
}
