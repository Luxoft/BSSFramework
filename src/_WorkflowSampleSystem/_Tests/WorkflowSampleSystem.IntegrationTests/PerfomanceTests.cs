using System;
using System.Linq;
using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven.BLL;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.IntegrationTests.__Support.TestData;

namespace WorkflowSampleSystem.IntegrationTests
{
    [TestClass]
    public class PerfomanceTests : TestBase
    {
        [TestMethod]
        public void GetEmployee_ToManyFilterParameters_CheckTimeTest()
        {
            var preEvaluate = this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context => context.Logics.Employee.GetUnsecureQueryable().First());

            var task = System.Threading.Tasks.Task.Run(() =>
                                                           this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
                                                           {
                                                               Expression<Func<Employee, bool>> filter = z => false;

                                                               var resultFilter = Enumerable.Range(0, 1000)
                                                                   .Select(number => (Expression<Func<Employee, bool>>)(z => z.Age == number && z.CellPhone == number.ToString()))
                                                                   .Aggregate(filter, (prev, current) => prev.BuildOr(current));

                                                               return context.Logics.Employee.GetUnsecureQueryable().Where(resultFilter).ToList();
                                                           }));

            var result = task.Wait(TimeSpan.FromSeconds(3));

            Assert.IsTrue(result);

            Assert.IsNotNull(preEvaluate);
        }
    }
}
