using Framework.Relations;

namespace SampleSystem.Domain.Directories;

public class BusinessUnitTypeLinkWithTransferTo :
        AuditPersistentDomainObjectBase,
        IDetail<BusinessUnitType>
{
    private BusinessUnitType businessUnitType;

    private BusinessUnitType transferTo;

    public BusinessUnitTypeLinkWithTransferTo()
    {
    }

    public BusinessUnitTypeLinkWithTransferTo(BusinessUnitType businessUnitType)
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

    public virtual BusinessUnitType TransferTo
    {
        get => this.transferTo;
        set => this.transferTo = value;
    }

    BusinessUnitType IDetail<BusinessUnitType>.Master => this.businessUnitType;
}
