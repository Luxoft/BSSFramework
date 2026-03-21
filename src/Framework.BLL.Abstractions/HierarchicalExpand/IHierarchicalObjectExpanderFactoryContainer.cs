using HierarchicalExpand;

namespace Framework.BLL.HierarchicalExpand;

public interface IHierarchicalObjectExpanderFactoryContainer
{
    IHierarchicalObjectExpanderFactory HierarchicalObjectExpanderFactory { get; }
}
