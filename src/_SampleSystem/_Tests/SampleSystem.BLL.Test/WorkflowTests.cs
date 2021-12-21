using System.Linq;
using System.Runtime.Serialization;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Workflow.Generated.DTO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.WebApiCore.Controllers;

namespace SampleSystem.BLL.Test
{
    [TestClass]
    public class WorkflowTests
    {
        [TestMethod]
        public void TestMemoryLeakWorkflow()
        {
            var environment = TestServiceEnvironment.IntegrationEnvironment;

            foreach (var iteration in Enumerable.Range(0, 2))
            {
                environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, context =>
                {
                    var tsService = context.Workflow.GetTargetSystemService(typeof(Permission), true);

                    foreach (
                        var command in
                            context.Workflow.Logics.Command.GetObjectsBy(command => command.TargetSystem == tsService.TargetSystem))
                    {
                        var anonType = tsService.CommandTypeBuilder.GetAnonymousType(command);

                        continue;
                    }
                });
            }
        }

        [TestMethod]
        public void TestUploadWorkflow()
        {
            var environment = TestServiceEnvironment.IntegrationEnvironment;

            environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                foreach (var wf in context.Workflow.Logics.Workflow.GetObjectsBy(wf => wf.Owner == null))
                {
                    context.Workflow.Logics.Workflow.Remove(wf);

                    continue;
                }
            });

            {
                var wfFacade = environment.GetController<WorkflowSLJsonController>();

                var wf = new DataContractSerializer(typeof(WorkflowStrictDTO)).ReadFromFile<WorkflowStrictDTO>(@"NewApprovePermission.xml");

                var res = wfFacade.SaveWorkflow(wf);
            }
        }
    }
}
