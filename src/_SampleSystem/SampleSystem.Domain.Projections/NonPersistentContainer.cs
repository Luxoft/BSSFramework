using Framework.Core;

namespace SampleSystem.Domain.Projections;

public partial class NonPersistentContainer
{
    public override List<TestLocation> Locations { get; set; } = null!;

    public override Period[] PeriodArray { get; set; } = null!;

    public override TestBusinessUnit TestBU { get; set; } = null!;

    public override string TestString { get; set; } = null!;
}
