using Framework.DomainDriven.UnitTest.Mock.StubProxy;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.DomainDriven.UnitTest.MockTest;

[TestClass]
public class StubFactoryTest
{
    [TestMethod]
    public void DefaultRefProperty()
    {
        var proxy = StubProxyFactory.CreateStub<IInterface>(new DefaultInterfaceImplement(1, new DefaultRefValue("default")));

        Assert.AreEqual("default", proxy.RefValue.Name);

    }

    [TestMethod]
    public void StubRefProperty()
    {

        var proxy = StubProxyFactory.CreateStub<IInterface>(
                                                            new DefaultInterfaceImplement(1, new DefaultRefValue("default"))
                                                            ,z => z.OverrideProperty(q => q.RefValue, new DefaultRefValue("overrided")));

        Assert.AreEqual("overrided", proxy.RefValue.Name);

    }

    [TestMethod]
    public void StubPrimitiveProperty()
    {
        var proxy = StubProxyFactory.CreateStub<IInterface>(
                                                            new DefaultInterfaceImplement(1, new DefaultRefValue(""))
                                                            , z => z.OverrideProperty(q => q.Value1, 5));
        Assert.AreEqual(5, proxy.Value1);
    }

    [TestMethod]
    public void DefaultPrimitiveProperty()
    {
        var proxy = StubProxyFactory.CreateStub<IInterface>(new DefaultInterfaceImplement(1, new DefaultRefValue("")));
        Assert.AreEqual(1, proxy.Value1);
    }
}
