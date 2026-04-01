using Framework.Relations;

namespace SampleSystem.Domain;

public class BusinessUnitTypeLinkWithPossibleParent :
        AuditPersistentDomainObjectBase,
        IDetail<BusinessUnitType>
{
    private BusinessUnitType businessUnitType;

    private BusinessUnitType possibleParent;

    public BusinessUnitTypeLinkWithPossibleParent()
    {
    }

    public BusinessUnitTypeLinkWithPossibleParent(BusinessUnitType businessUnitType)
    {
        if (businessUnitType == null)
        {
            throw new ArgumentNullException(nameof(businessUnitType));
        }

        this.businessUnitType = businessUnitType;
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
