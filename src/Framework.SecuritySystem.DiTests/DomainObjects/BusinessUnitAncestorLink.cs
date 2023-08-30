using Framework.Persistent;

namespace Framework.SecuritySystem.DiTests;

public class BusinessUnitAncestorLink : PersistentDomainObjectBase,
                                        IModifiedHierarchicalAncestorLink<BusinessUnit, BusinessUnitToAncestorChildView, Guid>
{
    public BusinessUnit Ancestor { get; set; }

    public BusinessUnit Child { get; set; }
}
