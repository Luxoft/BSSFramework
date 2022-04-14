using System;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore.Controllers;

using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace SampleSystem.IntegrationTests.Workflow
{
    [TestClass]
    public class ApprovePermissionTests : TestBase
    {
        private OperationSimpleDTO approveOperation;

        private BusinessRoleIdentityDTO roleWithApprove;

        private BusinessRoleIdentityDTO approvingRole;

        private const string SuperUserWithApprove = "ApproveWfUser";

        private const string TestUserForApproving = "ApprovingWfUser";

        private WorkflowManager workflowManager;

        private ControllerEvaluator<AuthSLJsonController> authFacade;

        [TestInitialize]
        public void SetUp()
        {
            this.authFacade = this.GetAuthControllerEvaluator();

            this.approveOperation = this.authFacade.Evaluate(c => c.GetSimpleOperationByName(nameof(SampleSystemSecurityOperationCode.ApproveWorkflowOperation)));

            var approveRole = this.authFacade.Evaluate(c => c.SaveBusinessRole(new BusinessRoleStrictDTO
                                                                               {
                                                                                       Name = "Approve Role",
                                                                                       BusinessRoleOperationLinks =
                                                                                       {
                                                                                               new BusinessRoleOperationLinkStrictDTO { Operation = this.approveOperation.Identity }
                                                                                       }
                                                                               }));

            var approverPrincipal = this.authFacade.Evaluate(c => c.SavePrincipal(new PrincipalStrictDTO
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

            var approvingOperation = this.authFacade.Evaluate(c => c.GetSimpleOperationByName(nameof(SampleSystemSecurityOperationCode.ApprovingWorkflowOperation)));

            this.approvingRole = this.authFacade.Evaluate(c => c.SaveBusinessRole(new BusinessRoleStrictDTO
                                                              {
                                                                      Name = "Approving Role",
                                                                      BusinessRoleOperationLinks =
                                                                      {
                                                                              new BusinessRoleOperationLinkStrictDTO { Operation = approvingOperation.Identity }
                                                                      }
                                                              }));

            this.Environment.ServiceProvider.GetRequiredService<WorkflowManager>().Start();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.Environment.ServiceProvider.GetRequiredService<WorkflowManager>().Stop();
        }

        [TestMethod]
        public async Task CreatePermissionWithApprove_PermissionApproved()
        {
            // Arrange
            var wfController = this.GetControllerEvaluator<WorkflowController>(SuperUserWithApprove);

            // Act
            var approvingPrincipal = this.CreateTestPermission();

            var permissionIdentity = approvingPrincipal.Permissions.Single().Identity;

            var startedWf = wfController.WithIntegrationImpersonate().Evaluate(c => c.StartJob());
            var rootInstanceId = startedWf[permissionIdentity.Id];

            var wfObjects = await WaitToCompleteHelper.Retry(
                () => wfController.EvaluateAsync(c => c.GetMyApproveOperationWorkflowObjects(permissionIdentity)),
                res => !res.Any(),
                TimeSpan.FromSeconds(10));

            foreach (var wfObj in wfObjects)
            {
                await wfController.EvaluateAsync(c => c.ApproveOperation(wfObj.ApproveEventId));
            }

            var wiStatus = this.Environment.ServiceProvider.GetRequiredService<IPersistenceProvider>().WaitForWorkflowToComplete(rootInstanceId.ToString(), TimeSpan.FromSeconds(10));

            var postApprovePrincipal = this.authFacade.Evaluate(c => c.GetRichPrincipal(approvingPrincipal.Identity));

            // Assert

            approvingPrincipal.Permissions.Single().Status.Should().Be(PermissionStatus.Approving);
            wiStatus.Should().Be(WorkflowStatus.Complete);
            postApprovePrincipal.Permissions.Single().Status.Should().Be(PermissionStatus.Approved);
        }

        [TestMethod]
        public async Task CreatePermissionWithApprove_PermissionRejected()
        {
            // Arrange
            var wfController = this.GetControllerEvaluator<WorkflowController>(SuperUserWithApprove);

            // Act
            var approvingPrincipal = this.CreateTestPermission();

            var permissionIdentity = approvingPrincipal.Permissions.Single().Identity;

            var startedWf = wfController.WithIntegrationImpersonate().Evaluate(c => c.StartJob());
            var rootInstanceId = startedWf[permissionIdentity.Id];

            var wfObjects = await WaitToCompleteHelper.Retry(
                                                             () => wfController.EvaluateAsync(c => c.GetMyApproveOperationWorkflowObjects(permissionIdentity)),
                                                             res => !res.Any(),
                                                             TimeSpan.FromSeconds(10));

            foreach (var wfObj in wfObjects)
            {
                await wfController.EvaluateAsync(c => c.RejectOperation(wfObj.RejectEventId));
            }

            var wiStatus = this.Environment.ServiceProvider.GetRequiredService<IPersistenceProvider>().WaitForWorkflowToComplete(rootInstanceId.ToString(), TimeSpan.FromSeconds(10));

            var postApprovePrincipal = this.authFacade.Evaluate(c => c.GetRichPrincipal(approvingPrincipal.Identity));

            // Assert

            approvingPrincipal.Permissions.Single().Status.Should().Be(PermissionStatus.Approving);
            wiStatus.Should().Be(WorkflowStatus.Complete);
            postApprovePrincipal.Permissions.Single().Status.Should().Be(PermissionStatus.Rejected);
        }

        private PrincipalRichDTO CreateTestPermission()
        {
            var approvingPrincipal = this.authFacade.Evaluate(c => c.SavePrincipal(new PrincipalStrictDTO
            {
                    Name = TestUserForApproving,
                    Permissions =
                    {
                            new PermissionStrictDTO
                            {
                                    Role = this.approvingRole,
                            }
                    }
            }));

            return this.authFacade.Evaluate(c => c.GetRichPrincipal(approvingPrincipal));
        }
    }
}
