using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Workflow.Domain;
using System.Reflection;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL.Tests.Unit.FiltersTests
{
    [TestClass]
    public class BaseFilterModelTest : BaseUnitTest
    {
        protected bool IsGenericFilterType(Type t)
        {
            var genArgs = t.GetGenericArguments();
            if (genArgs.Length == 1 && typeof(DomainObjectFilterModel<>).MakeGenericType(genArgs).IsAssignableFrom(t))
            {
                return true;
            }

            return t.BaseType != null && this.IsGenericFilterType(t.BaseType);
        }
        [Ignore]
        [TestMethod]
        public void ToFilterExpressionGenericTest()
        {
            var assembly = Assembly.GetAssembly(typeof (Framework.Workflow.Domain.RoleRootFilterModel));
            var filterTypes = assembly.GetTypes().Where(t => this.IsGenericFilterType(t)).ToArray();

            var context = this.GetContext();
            var workflow = context.Logics.Workflow.Create(new WorkflowCreateModel());

            foreach (var filterType in filterTypes)
            {
                if (filterType.ContainsGenericParameters)
                    continue;

                var createdFilter = Activator.CreateInstance(filterType);
                var wfProperty = filterType.GetProperty("Workflow");
                if (wfProperty != null)
                {
                    wfProperty.SetValue(createdFilter, workflow, null);
                }

                var result = filterType.GetMethod("ToFilterExpression").Invoke(createdFilter, null);
                //var result = ((DomainObjectFilterModel<WorkflowItemBase>)createdFilter).ToFilterExpression();

                Assert.IsNotNull(result);

                var compileMethod = result.GetType().GetMethod("Compile", new Type[0]);
                Assert.IsNotNull(compileMethod);

                Assert.IsNotNull(compileMethod.Invoke(result, null));
            }
        }


    }
}
