using System;
using System.Linq;
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
            var wfController = this.GetControllerEvaluator<WorkflowController>(SuperUserWithApprove);

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
            var permissionIdentity = preApprovePrincipal.Permissions.Single().Identity;

            var startedWf = wfController.WithIntegrationImpersonate().Evaluate(c => c.StartJob());
            var rootInstanceId = startedWf[permissionIdentity.Id];

            var wfObjects = await WaitToCompleteHelper.Retry(
                () => wfController.EvaluateAsync(c => c.GetMyApproveOperationWorkflowObjects(permissionIdentity)),
                res => !res.Any(),
                TimeSpan.FromSeconds(10));

            foreach (var wfObj in wfObjects)
            {
                await wfController.EvaluateAsync(c => c.ApproveOperation(wfObj));
            }

            var wiStatus = this.Environment.ServiceProvider.GetRequiredService<IPersistenceProvider>().WaitForWorkflowToComplete(rootInstanceId.ToString(), TimeSpan.FromSeconds(10));

            var postApprovePrincipal = authFacade.Evaluate(c => c.GetRichPrincipal(approvingPrincipal));

            // Assert

            preApprovePrincipal.Permissions.Single().Status.Should().Be(PermissionStatus.Approving);
            wiStatus.Should().Be(WorkflowStatus.Complete);
            postApprovePrincipal.Permissions.Single().Status.Should().Be(PermissionStatus.Approved);
        }
    }
}
