using Framework.DomainDriven;

using GenericQueryable;

using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class NHibFetchTests : TestBase
{
    [TestMethod]
    public void TestPropCollection_TestPassed()
    {
        this.Evaluate(
            DBSessionMode.Read,
            ctx =>
            {
                var z = ctx.Logics.Employee.GetUnsecureQueryable().WithFetch(f => f.Fetch(e => e.CellPhones)).ToList();

                return;
            });
    }
}
