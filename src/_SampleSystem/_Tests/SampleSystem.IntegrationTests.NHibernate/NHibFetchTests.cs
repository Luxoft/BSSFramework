using Framework.Application;
using Framework.Database;

using Anch.GenericQueryable;

using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests;

public class NHibFetchTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
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
