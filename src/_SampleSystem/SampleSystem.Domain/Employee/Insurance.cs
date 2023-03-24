using System.ComponentModel;

using Framework.Persistent;

namespace SampleSystem.Domain;

public class Insurance : DomainObjectBase
{
    [DetailRole(true)]
    public InsuranceDetail Details { get; set; }

    public Insurance Self { get; set; }

    public Employee Employee { get; set; }

    public int WorkExperience { get; set; }

    [DefaultValue(12)]
    public int DurationMonths { get; set; }

    public decimal TotalIncome { get; set; }

    public bool UseFullAnnualRemainder { get; set; }
}
