using Framework.Relations;
using Framework.Restriction;
using Framework.Validation.Attributes;

namespace SampleSystem.Domain.Employee;

public abstract class EmployeeCellPhoneBase : AuditPersistentDomainObjectBase, IDetail<Employee>
{
    protected readonly Employee employee = null!;

    protected string countryCode = "";
    protected string cityCode = "";
    protected string number = "";

    protected string fullNumber = "";

    protected EmployeeCellPhoneBase()
    {
    }

    protected EmployeeCellPhoneBase(Employee employee)
    {
        this.employee = employee ?? throw new ArgumentNullException(nameof(employee));
    }

    public virtual Employee Employee => this.employee;

    [Required]
    [NumberAlphabetValidator]
    [MaxLength(3)]
    public virtual string CountryCode
    {
        get => this.countryCode;
        set => this.countryCode = value;
    }

    [Required]
    [NumberAlphabetValidator]
    [MaxLength(5)]
    public virtual string CityCode
    {
        get => this.cityCode;
        set => this.cityCode = value;
    }

    [Required]
    [NumberAlphabetValidator]
    [MaxLength(7)]
    public virtual string Number
    {
        get => this.number;
        set => this.number = value;
    }

    [Required]
    [NumberAlphabetValidator(ExternalChars = "+()")]
    [MaxLength(18)]
    public virtual string FullNumber
    {
        get => this.fullNumber;
        protected internal set => this.fullNumber = value;
    }

    Employee IDetail<Employee>.Master => this.employee;
}
