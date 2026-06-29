using System.ComponentModel;

using Framework.Relations;

namespace SampleSystem.Domain.Employee;

public class Insurance : DomainObjectBase
{
    [DetailRole(true)]
    public InsuranceDetail Details { get; set; } = null!;

    public Insurance? Self { get; set; }

    public Employee Employee { get; set; } = null!;

    public int WorkExperience { get; set; }

    [DefaultValue(12)]
    public int DurationMonths { get; set; }

    public decimal TotalIncome { get; set; }

    public bool UseFullAnnualRemainder { get; set; }
}

