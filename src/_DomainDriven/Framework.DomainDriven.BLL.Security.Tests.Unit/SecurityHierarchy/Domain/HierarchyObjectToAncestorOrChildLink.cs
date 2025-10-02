namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;

public class HierarchyObjectToAncestorOrChildLink : IHierarchicalToAncestorOrChildLink<HierarchyObject, Guid>
{
    public HierarchyObject ChildOrAncestor { get; private set; }

    public HierarchyObject Source { get; private set; }
}
