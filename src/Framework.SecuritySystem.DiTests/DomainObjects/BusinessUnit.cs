using Framework.Persistent;

namespace Framework.SecuritySystem.DiTests;

public class BusinessUnit : PersistentDomainObjectBase,
                            ISecurityContext,
                            IDenormalizedHierarchicalPersistentSource<BusinessUnitAncestorLink, BusinessUnitToAncestorChildView, BusinessUnit, Guid>,
                            IModifiedIHierarchicalLevelObject
{
    public BusinessUnit Parent { get; set; }

    public IEnumerable<BusinessUnit> Children { get; set; } = new List<BusinessUnit>();

    public int DeepLevel { get; set; }
}
