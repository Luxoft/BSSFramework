using Framework.Application;
using Framework.Database;

using GenericQueryable;

using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

public class NHibFetchTests : TestBase
{
    [Fact]
    public void TestPropCollection_TestPassed() =>
        this.Evaluate(
            DBSessionMode.Read,
            ctx =>
            {
                var z = ctx.Logics.Employee.GetUnsecureQueryable().WithFetch(f => f.Fetch(e => e.CellPhones)).ToList();

                return;
            });
}
