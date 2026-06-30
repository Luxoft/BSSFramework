using Framework.Relations;

namespace SampleSystem.Domain.Directories;

public class BusinessUnitTypeLinkWithPossibleParent :
        AuditPersistentDomainObjectBase,
        IDetail<BusinessUnitType>
{
    private BusinessUnitType businessUnitType = null!;

    private BusinessUnitType possibleParent = null!;

    public BusinessUnitTypeLinkWithPossibleParent()
    {
    }

    public BusinessUnitTypeLinkWithPossibleParent(BusinessUnitType businessUnitType)
    {
        this.businessUnitType = businessUnitType ?? throw new ArgumentNullException(nameof(businessUnitType));
        this.businessUnitType.AddDetail(this);
    }

    [IsMaster]
    public virtual BusinessUnitType BusinessUnitType
    {
        get => this.businessUnitType;
        set => this.businessUnitType = value;
    }

    public virtual BusinessUnitType PossibleParent
    {
        get => this.possibleParent;
        set => this.possibleParent = value;
    }

    BusinessUnitType IDetail<BusinessUnitType>.Master => this.businessUnitType;
}

