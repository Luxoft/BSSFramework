#pragma warning disable S1481 // Unused local variables should be removed

using System;
using System.Diagnostics;

using Framework.DomainDriven;
using Framework.Transfering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Workflow.BLL.Tests.Unit.DomainTests
{
    [TestClass]
    public class FetchTests : BaseUnitTest
    {
        [TestMethod]
        public void TestFetchs()
        {
            var service = FetchService<Framework.Workflow.Domain.PersistentDomainObjectBase>.Mixed;

            var container = service.GetContainer<Framework.Workflow.Domain.Definition.Workflow>(MainDTOType.FullDTO);

            return;
        }
    }
}

#pragma warning restore S1481 // Unused local variables should be removed
