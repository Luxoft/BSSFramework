using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using Framework.Authorization.ApproveWorkflow;
using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven.BLL;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore.Controllers;

using WorkflowCore.Interface;
using WorkflowCore.Models;

using DBSessionMode = Framework.DomainDriven.BLL.DBSessionMode;

namespace SampleSystem.IntegrationTests.Workflow
{
    [TestClass]
    public class ApprovePermissionTests : TestBase
    {
        private OperationSimpleDTO approveOperation;

        private BusinessRoleIdentityDTO roleWithApprove;

        private const string SuperUserWithApprove = "ApproveWfUser";

        [TestInitialize]
        public void SetUp()
        {
            var authFacade = this.GetAuthControllerEvaluator();

            this.approveOperation = authFacade.Evaluate(c => c.GetSimpleOperationByName(nameof(SampleSystemSecurityOperationCode.ApproveWorkflowOperation)));

            var approveRole = authFacade.Evaluate(c => c.SaveBusinessRole(new BusinessRoleStrictDTO
                                                                          {
                                                                                  Name = "Approve Role",
                                                                                  BusinessRoleOperationLinks =
                                                                                  {
                                                                                          new BusinessRoleOperationLinkStrictDTO { Operation = this.approveOperation.Identity }
                                                                                  }
                                                                          }));

            var approverPrincipal = authFacade.Evaluate(c => c.SavePrincipal(new PrincipalStrictDTO
                                                                             {
                                                                                     Name = SuperUserWithApprove,
                                                                                     Permissions =
                                                                                     {
                                                                                             new PermissionStrictDTO
                                                                                             {
                                                                                                     Role = approveRole,
                                                                                             }
                                                                                     }
                                                                             }));
        }

        [TestMethod]
        public async Task CreatePermission_WorkflowPassed()
        {
            // Arrange
            var testUserForApproving = "ApprovingWfUser";

            this.Environment.ServiceProvider.GetRequiredService<WorkflowManager>().Start();

            var authFacade = this.GetControllerEvaluator<AuthSLJsonController>();
            var wfController = this.GetControllerEvaluator<WorkflowController>();

            //var workflowHost = this.GetWorkflowControllerEvaluator();

            var approvingOperation = authFacade.Evaluate(c => c.GetSimpleOperationByName(nameof(SampleSystemSecurityOperationCode.ApprovingWorkflowOperation)));

            var approvingRole = authFacade.Evaluate(c => c.SaveBusinessRole(new BusinessRoleStrictDTO
                                                                          {
                                                                                  Name = "Approving Role",
                                                                                  BusinessRoleOperationLinks =
                                                                                  {
                                                                                          new BusinessRoleOperationLinkStrictDTO { Operation = approvingOperation.Identity }
                                                                                  }
                                                                          }));

            // Act
            var approvingPrincipal = authFacade.Evaluate(c => c.SavePrincipal(new PrincipalStrictDTO
                                                                              {
                                                                                      Name = testUserForApproving,
                                                                                      Permissions =
                                                                                      {
                                                                                              new PermissionStrictDTO
                                                                                              {
                                                                                                      Role = approvingRole,
                                                                                              }
                                                                                      }
                                                                              }));

            var preApprovePrincipal = authFacade.Evaluate(c => c.GetRichPrincipal(approvingPrincipal));
            var permissionId = preApprovePrincipal.Permissions.Single().Id;
            var permissionIdStr = permissionId.ToString();


            var startedWf = wfController.Evaluate(c => c.StartJob());
            var rootInstanceId = startedWf[permissionId];

            var operationWfInstances = WaitToCompleteHelper.Retry(() => this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, ctx =>
            {
                var bll = ctx.Logics.Default.Create<WorkflowCoreInstance>();

                var instances = bll.GetUnsecureQueryable().Where(wi =>
                    wi.Data.Contains(permissionIdStr)
                 && wi.WorkflowDefinitionId == nameof(__ApproveOperation_Workflow)
                 && wi.Status == WorkflowStatus.Runnable);

                return instances.ToList(wi => new { wi.Id, wi.WorkflowDefinitionId, wi.Status } );
            }), res => !res.Any(), TimeSpan.FromSeconds(100));

            var host = this.Environment.ServiceProvider.GetRequiredService<IWorkflowHost>();

            foreach (var wi in operationWfInstances)
            {
                await host.PublishEvent("Approve_Event", wi.Id.ToString(), new object());
            }

            var wiStatus = host.PersistenceStore.WaitForWorkflowToComplete(rootInstanceId.ToString(), TimeSpan.FromSeconds(200));

            var postApprovePrincipal = authFacade.Evaluate(c => c.GetRichPrincipal(approvingPrincipal));

            // Assert

            preApprovePrincipal.Permissions.Single().Status.Should().Be(PermissionStatus.Approving);

            wiStatus.Should().Be(WorkflowStatus.Complete);

            postApprovePrincipal.Permissions.Single().Status.Should().Be(PermissionStatus.Approved);

            return;
        }
    }
}
public static class WaitToCompleteHelper
{
    public static T Retry<T>(Func<T> func, Func<T, bool> continueCondition, TimeSpan timeOut)
    {
        var res = func();
        var counter = 0;
        while (continueCondition(res) && counter < (timeOut.TotalMilliseconds / 100))
        {
            Thread.Sleep(100);
            counter++;
            res = func(); ;
        }

        return res;
    }

    public static WorkflowStatus WaitForWorkflowToComplete(this IPersistenceProvider persistenceProvider, string workflowId, TimeSpan timeOut)
    {
        return Retry (() => persistenceProvider.GetStatus(workflowId), status => status == WorkflowStatus.Runnable, timeOut);
    }

    public static WorkflowStatus GetStatus(this IPersistenceProvider persistenceProvider, string workflowId)
    {
        var instance = persistenceProvider.GetWorkflowInstance(workflowId).Result;
        return instance.Status;
    }
}
