using Framework.Relations;

using SampleSystem.Domain.Enums;

namespace SampleSystem.Domain.Directories;

public class BusinessUnitTypeLinkWithPossibleFinancialProjectType :
        AuditPersistentDomainObjectBase,
        IDetail<BusinessUnitType>
{
    private BusinessUnitType businessUnitType = null!;

    private FinancialProjectType financialProjectType;

    public BusinessUnitTypeLinkWithPossibleFinancialProjectType()
    {
    }

    public BusinessUnitTypeLinkWithPossibleFinancialProjectType(BusinessUnitType businessUnitType)
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

    public virtual FinancialProjectType FinancialProjectType
    {
        get => this.financialProjectType;
        set => this.financialProjectType = value;
    }

    BusinessUnitType IDetail<BusinessUnitType>.Master => this.businessUnitType;
}

