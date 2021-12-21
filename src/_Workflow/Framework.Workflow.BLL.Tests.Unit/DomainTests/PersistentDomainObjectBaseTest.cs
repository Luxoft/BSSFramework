using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Framework.Workflow.Domain;

namespace Framework.Workflow.BLL.Tests.Unit.DomainTests
{
    [TestClass]
    public class PersistentDomainObjectBaseTest : BaseUnitTest
    {
        //[TestMethod]
        //public void TestIsNewTrue()
        //{
        //    //var mocks = new MockRepository();
        //    //var persistent = mocks.StrictMock<PersistentDomainObjectBase>();
        //    //Expect.Call(persistent.Id.Returns(Guid.Empty);
        //    //mocks.ReplayAll();
        //    //Assert.IsTrue(persistent.IsNew);
        //    //mocks.VerifyAll();

        //    //var persistent = GetStub<PersistentDomainObjectBase>();
        //    //persistent.Id.Returns(Guid.Empty);
        //    //Assert.IsTrue(persistent.IsNew);

        //    var mocks = new MockRepository();
        //    var persistent = mocks.Stub<PersistentDomainObjectBase>();

        //    persistent.Id.Returns(Guid.Empty);

        //    Assert.IsTrue(persistent.IsNew);
        //}

        //[TestMethod]
        //public void TestIsNewFalse()
        //{
        //    var persistent = GetStub<PersistentDomainObjectBase>();
        //    persistent.Id = Guid.NewGuid();
        //    Assert.IsFalse(persistent.IsNew);
        //}
    }
}
