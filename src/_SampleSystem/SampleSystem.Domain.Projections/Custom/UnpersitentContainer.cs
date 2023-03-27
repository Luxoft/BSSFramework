using Framework.Core;

namespace SampleSystem.Domain.Projections;

public partial class UnpersitentContainer
{
    public override List<TestLocation> Locations { get; set; }

    public override Period[] PeriodArray { get; set; }

    public override TestBusinessUnit TestBU { get; set; }

    public override string TestString { get; set; }
}
