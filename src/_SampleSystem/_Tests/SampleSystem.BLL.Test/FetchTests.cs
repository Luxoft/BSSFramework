using Framework.DomainDriven;
using Framework.Transfering;
using Framework.Workflow.Domain.Definition;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SampleSystem.BLL.Test
{
    [TestClass]
    public class FetchTests
    {
        [TestMethod]
        public void TestFetchs()
        {
            var factory = new ExpandFetchPathFactory(typeof(Framework.Workflow.Domain.PersistentDomainObjectBase));

            var c = factory.Create(typeof (Task), ViewDTOType.RichDTO);
        }
    }
}
