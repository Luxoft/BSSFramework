using Framework.Core;

namespace SampleSystem.Domain.Projections;

public partial class TestLocationCollectionProperties
{
    public override Guid[] Child_Identities { get; } = null!;

    public override Period[] Child_Periods { get; } = null!;

    public override DateTime[] Date_Intervals { get; } = null!;

    public override string[] Security_Codes { get; } = null!;
}
