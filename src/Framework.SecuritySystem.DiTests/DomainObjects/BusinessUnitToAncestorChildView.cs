using Framework.Persistent;

namespace Framework.SecuritySystem.DiTests;

public class BusinessUnitToAncestorChildView : PersistentDomainObjectBase, IHierarchicalToAncestorOrChildLink<BusinessUnit, Guid>
{
    public virtual BusinessUnit ChildOrAncestor { get; set; }

    public virtual BusinessUnit Source { get; set; }
}
