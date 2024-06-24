using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Validation;

namespace SampleSystem.Domain;

[BLLViewRole]
[CustomName("Manager Commissions")]
public class BusinessUnitManagerCommissionLink : AuditPersistentDomainObjectBase, IDetail<BusinessUnit>
{
    private readonly BusinessUnit businessUnit;

    private decimal commission;

    private Employee manager;

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
            Employee manager,
            Period period)
            : this(businessUnit)
    {
        if (businessUnit == null) throw new ArgumentNullException(nameof(businessUnit));
        if (manager == null) throw new ArgumentNullException(nameof(manager));

        this.commission = commission;
        this.manager = manager;
        this.period = period;
    }

    [Framework.Restriction.Required]
    [CustomName("Financial Business Unit")]
    public virtual BusinessUnit BusinessUnit
    {
        get
        {
            return this.businessUnit;
        }

        set
        {
            this.SetValueSafe(x => x.businessUnit, value);
        }
    }

    [Framework.Restriction.Required]
    [Framework.Restriction.UniqueElement]
    public virtual Employee Manager
    {
        get
        {
            return this.manager;
        }

        set
        {
            this.manager = value;
        }
    }

    [Percent]
    [SignValidator(SignType.Positive | SignType.Negative)]
    [CustomName("Commission, %")]
    public virtual decimal Commission
    {
        get { return this.commission; }
        set { this.commission = value; }
    }

    public virtual Period Period
    {
        get { return this.period; }
        set { this.period = value; }
    }

    [Framework.Restriction.UniqueElement]
    [ExpandPath("Period")]
    [CustomSerialization(CustomSerializationMode.Ignore)]
    public virtual DateTime StartDate
    {
        get { return this.Period.StartDate; }
    }

    BusinessUnit IDetail<BusinessUnit>.Master
    {
        get
        {
            return this.BusinessUnit;
        }
    }

    public override string ToString()
    {
        return $"BusinessUnit: {this.BusinessUnit}, Manager: {this.Manager}, Period: {this.Period}";
    }
}
