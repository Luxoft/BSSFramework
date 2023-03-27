using Framework.Persistent;

namespace Framework.SecuritySystem.DiTests;

public class BusinessUnit : PersistentDomainObjectBase, ISecurityContext, IHierarchicalPersistentDomainObjectBase<BusinessUnit, Guid>
{
    public BusinessUnit Parent { get; set; }

    public IEnumerable<BusinessUnit> Children { get; set; } = new List<BusinessUnit>();
}
