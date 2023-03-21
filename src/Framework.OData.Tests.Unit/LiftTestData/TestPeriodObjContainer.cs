using Framework.Core;
using Framework.Persistent;

namespace Framework.OData.Tests.Unit;

public class TestPeriodObjContainer
{
    [ExpandPath("InnerObj.Period")]
    public Period? Period => this.InnerObj?.Period;

    public TestPeriodObj InnerObj { get; set; }
}
