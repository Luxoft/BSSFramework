using Framework.BLL.Domain;
using Framework.BLL.Domain.Attributes.Round;
using Framework.BLL.Domain.Persistent.Attributes;
using Framework.BLL.Domain.Serialization;
using Framework.BLL.Domain.ServiceRole;
using Framework.Core;
using Framework.Relations;
using Framework.Restriction;
using Framework.Validation;

namespace SampleSystem.Domain.BU;

[BLLViewRole]
[CustomName("Manager Commissions")]
public class BusinessUnitManagerCommissionLink : AuditPersistentDomainObjectBase, IDetail<BusinessUnit>
{
    private readonly BusinessUnit businessUnit;

    private decimal commission;

    private Employee.Employee manager;

    private Period period;

    public BusinessUnitManagerCommissionLink()
    {
    }

    public BusinessUnitManagerCommissionLink(BusinessUnit businessUnit)
    {
        if (businessUnit == null)
        {
            throw new ArgumentNullException(nameof(businessUnit));
        }

        this.businessUnit = businessUnit;

        businessUnit.AddDetail(this);
    }

    public BusinessUnitManagerCommissionLink(
            BusinessUnit businessUnit,
            decimal commission,
            Employee.Employee manager,
            Period period)
            : this(businessUnit)
    {
        if (businessUnit == null) throw new ArgumentNullException(nameof(businessUnit));
        if (manager == null) throw new ArgumentNullException(nameof(manager));

        this.commission = commission;
        this.manager = manager;
        this.period = period;
    }

    [Required]
    [CustomName("Financial Business Unit")]
    public virtual BusinessUnit BusinessUnit
    {
        get => this.businessUnit;
        set => this.SetValueSafe(x => x.businessUnit, value);
    }

    [Required]
    [UniqueElement]
    public virtual Employee.Employee Manager
    {
        get => this.manager;
        set => this.manager = value;
    }

    [Percent]
    [SignValidator(SignType.Positive | SignType.Negative)]
    [CustomName("Commission, %")]
    public virtual decimal Commission
    {
        get => this.commission;
        set => this.commission = value;
    }

    public virtual Period Period
    {
        get => this.period;
        set => this.period = value;
    }

    [UniqueElement]
    [ExpandPath(nameof(Period))]
    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual DateTime StartDate => this.Period.StartDate;

    BusinessUnit IDetail<BusinessUnit>.Master => this.BusinessUnit;

    public override string ToString() => $"BusinessUnit: {this.BusinessUnit}, Manager: {this.Manager}, Period: {this.Period}";
}
