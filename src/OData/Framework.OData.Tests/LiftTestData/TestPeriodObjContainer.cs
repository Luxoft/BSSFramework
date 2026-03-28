using Framework.BLL.Domain.Persistent.Attributes;
using Framework.Core;

namespace Framework.OData.Tests.LiftTestData;

public class TestPeriodObjContainer
{
    [ExpandPath("InnerObj.Period")]
    public Period? Period => this.InnerObj?.Period;

    public TestPeriodObj InnerObj { get; set; }
}
