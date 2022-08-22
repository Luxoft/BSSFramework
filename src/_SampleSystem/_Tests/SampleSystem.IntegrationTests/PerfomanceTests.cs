using System;
using System.Linq;
using System.Linq.Expressions;
using Automation.ServiceEnvironment;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class PerfomanceTests : TestBase
    {
        [TestMethod]
        public void GetEmployee_ToManyFilterParameters_CheckTimeTest()
        {
            var preEvaluate = this.Evaluate(DBSessionMode.Write, context => context.Logics.Employee.GetUnsecureQueryable().First());

            var task = System.Threading.Tasks.Task.Run(() =>
                                                           this.Evaluate(DBSessionMode.Write, context =>
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
