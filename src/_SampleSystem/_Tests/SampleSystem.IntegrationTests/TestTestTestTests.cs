using Framework.DomainDriven.BLL;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class TestTestTestTests : TestBase
{
    [TestMethod]
    public void TestSecurityObjItem_LoadedByDependencySecurity()
    {
        this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, (ctx, session) =>
        {
            session.Flush();

            var emp = ctx.Logics.Employee.GetFullList();

            session.Flush();

            //var x = ctx.Logics.TestImmutableObj;

            return;
        });
    }
}
