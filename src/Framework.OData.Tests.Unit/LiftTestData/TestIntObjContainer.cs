using Framework.Persistent;

namespace Framework.OData.Tests.Unit
{
    public class TestIntObjContainer
    {
        [ExpandPath("InnerObj.Int")]
        public int? Int => this.InnerObj?.Int;

        public TestIntObj InnerObj { get; set; }
    }
}
