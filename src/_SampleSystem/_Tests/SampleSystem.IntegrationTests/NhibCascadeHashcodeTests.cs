using Automation.ServiceEnvironment;

using Framework.DomainDriven;
using Framework.Persistent;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class NhibCascadeHashcodeTests : TestBase
{
    [TestMethod]
    public void TestCascade()
    {
        this.EvaluateWrite(
            c =>
            {
                var rootObj = new Example1();

                var childObj = new Example2(rootObj);

                //c.Logics.Default.Create<Example1>().Save(rootObj);

                //c.Logics.Default.Create<Example2>().Save(childObj);

                rootObj.RemoveDetail(childObj);

                return;
            });
    }
}
