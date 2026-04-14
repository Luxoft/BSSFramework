using Framework.Core;
using Framework.Relations;
using Framework.Restriction;
using Framework.Validation;
using Framework.Validation.Attributes;

namespace SampleSystem.Domain.Employee;

public class EmployeeCellPhoneBase : AuditPersistentDomainObjectBase, IDetail<Employee>
{
    protected readonly Employee employee;

    protected string countryCode;
    protected string cityCode;
    protected string number;

    protected string fullNumber;

    protected EmployeeCellPhoneBase()
    {
    }

    protected EmployeeCellPhoneBase(Employee employee)
    {
        if (employee == null)
        {
            throw new ArgumentNullException(nameof(employee));
        }

        this.employee = employee;
    }

    public virtual Employee Employee => this.employee;

    [Required]
    [NumberAlphabetValidator]
    [MaxLength(3)]
    public virtual string CountryCode
    {
        get => this.countryCode.TrimNull();
        set => this.countryCode = value.TrimNull();
    }

    [Required]
    [NumberAlphabetValidator]
    [MaxLength(5)]
    public virtual string CityCode
    {
        get => this.cityCode.TrimNull();
        set => this.cityCode = value.TrimNull();
    }

    [Required]
    [NumberAlphabetValidator]
    [MaxLength(7)]
    public virtual string Number
    {
        get => this.number.TrimNull();
        set => this.number = value.TrimNull();
    }

    [Required]
    [NumberAlphabetValidator(ExternalChars = "+()")]
    [MaxLength(18)]
    public virtual string FullNumber
    {
        get => this.fullNumber.TrimNull();
        protected internal set => this.fullNumber = value.TrimNull();
    }

    Employee IDetail<Employee>.Master => this.employee;
}
