namespace SampleSystem.Domain.Models.Custom;

public class DateModel : DomainObjectBase
{
    public int Year { get; set; }

    public int? Month { get; set; }

    public int? Day { get; set; }

    public DateTime StartDate() => new DateTime(this.Year, this.Month.GetValueOrDefault(1), this.Day.GetValueOrDefault(1));
}
