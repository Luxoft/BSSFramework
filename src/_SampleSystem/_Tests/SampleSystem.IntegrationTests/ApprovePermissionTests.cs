using System;
using System.Linq;

using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore.Controllers;

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

        private SampleSystemServiceEnvironment WorkflowEnvironment { get; } = WorkflowTestServiceEnvironment.Default;

        [TestMethod]
        public void CreatePermission_WorkflowPassed()
        {
            // Arrange
            var testUserForApproving = "ApprovingWfUser";

            var authFacade = this.WorkflowEnvironment.ServiceProvider.GetDefaultControllerEvaluator<AuthSLJsonController>();

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

            //var taskInstance = workflowFacade.Evaluate(c => c.GetSimpleTaskInstancesByRootFilter(new TaskInstanceRootFilterModelStrictDTO { DomainObjectId = this.approveOperation.Id })).Single();

            //workflowFacade.WithImpersonate(SuperUserWithApprove).Evaluate(c => c.ExecuteCommand(new ExecuteCommandRequestStrictDTO
            //{
            //        Command = this.approveCommand.Identity,
            //        TaskInstance = taskInstance.Identity,
            //        Parameters =
            //        {
            //                new ExecuteCommandRequestParameterStrictDTO { Definition = this.approveCommandCommentParameter.Identity, Value = "Ok!" },
            //                new ExecuteCommandRequestParameterStrictDTO { Definition = this.approveCommandPotentialApproversParameter.Identity, Value = "Vasia" }
            //        }
            //}));

            var postApprovePrincipal = authFacade.Evaluate(c => c.GetRichPrincipal(approvingPrincipal));

            // Assert

            preApprovePrincipal.Permissions.Single().Status.Should().Be(PermissionStatus.Approving);

            postApprovePrincipal.Permissions.Single().Status.Should().Be(PermissionStatus.Approved);

            return;
        }
    }
}
