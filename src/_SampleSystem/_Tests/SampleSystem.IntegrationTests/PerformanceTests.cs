using System.Linq.Expressions;

using Anch.Core;
using Anch.Testing.Xunit;

using Framework.BLL;
using Framework.Database;

using SampleSystem.Domain.Employee;
using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests;

public abstract class PerformanceTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [AnchFact]
    public void GetEmployee_ToManyFilterParameters_CheckTimeTest(CancellationToken ct)
    {
        var preEvaluate = this.Evaluate(DBSessionMode.Write, context => context.Logics.Employee.GetUnsecureQueryable().First());

        var task = Task.Run(() =>
                                this.Evaluate(
                                    DBSessionMode.Write,
                                    context =>
                                    {
                                        Expression<Func<Employee, bool>> filter = z => false;

                                        var resultFilter = Enumerable.Range(0, 100)
                                                                     .Select(number =>
                                                                                 (Expression<Func<Employee, bool>>)(z => z.Age == number
                                                                                             && z.CellPhone == number.ToString()))
                                                                     .Aggregate(filter, (prev, current) => prev.BuildOr(current));

                                        return context.Logics.Employee.GetUnsecureQueryable().Where(resultFilter).ToList();
                                    }),
                            ct);

        var result = task.Wait(TimeSpan.FromSeconds(3), ct);

        Xunit.Assert.True(result);
    }
}
