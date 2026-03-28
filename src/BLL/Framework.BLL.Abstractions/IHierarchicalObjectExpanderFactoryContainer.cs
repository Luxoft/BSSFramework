using HierarchicalExpand;

namespace Framework.BLL;

public interface IHierarchicalObjectExpanderFactoryContainer
{
    IHierarchicalObjectExpanderFactory HierarchicalObjectExpanderFactory { get; }
}
