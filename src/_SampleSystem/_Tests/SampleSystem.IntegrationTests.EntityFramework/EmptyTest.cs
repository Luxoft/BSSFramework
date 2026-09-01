using Anch.Testing.Xunit;

using Framework.Application;
using Framework.Database;

using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests;

public class EmptyTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [AnchFact]
    public async Task Test1(CancellationToken ct)
    {
        // Arrange

        await this.EvaluateAsync(DBSessionMode.Write,
                                 async ctx =>
                                 {
                                     var v = ctx.Logics.BusinessUnit.GetFullList();
                                     return;
                                 }, ct);

        await this.EvaluateAsync(DBSessionMode.Write,
                                 async ctx =>
                                 {
                                     var v = ctx.Logics.BusinessUnit.GetFullList();
                                     return;
                                 }, ct);
    }
}
