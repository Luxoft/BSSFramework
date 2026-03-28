using Framework.BLL.Domain.Persistent.Attributes;

namespace Framework.OData.Tests.LiftTestData;

public class TestIntObjContainer
{
    [ExpandPath("InnerObj.Int")]
    public int? Int => this.InnerObj?.Int;

    public TestIntObj? InnerObj { get; set; }
}
