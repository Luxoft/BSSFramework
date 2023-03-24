using System;
using System.Linq;

using Framework.Core;

namespace SampleSystem.Domain.Projections;

public partial class TestLocationCollectionProperties
{
    public override Guid[] Child_Identities { get; }

    public override Period[] Child_Periods { get; }

    public override DateTime[] Date_Intervals { get; }

    public override SampleSystemSecurityOperationCode[] Security_Codes { get; }
}
