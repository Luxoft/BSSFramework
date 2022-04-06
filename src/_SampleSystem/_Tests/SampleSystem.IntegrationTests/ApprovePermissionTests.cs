using System;
using System.Linq;
using System.Runtime.Serialization;

using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.ServiceEnvironment;

namespace SampleSystem.IntegrationTests.Workflow
{
    [TestClass]
    public class ApprovePermissionTests : TestBase
    {
        private OperationSimpleDTO approveOperation;

        private BusinessRoleIdentityDTO roleWithApprove;

        private const string SuperUserWithApprove = "ApproveWfUser";


        //private WorkflowIdentityDTO wfIdent;

        //private CommandRichDTO approveCommand;

        //private CommandParameterRichDTO approveCommandCommentParameter;

        //private CommandParameterRichDTO approveCommandPotentialApproversParameter;


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

            //this.wfIdent = wfFacade.Evaluate(c => c.SaveWorkflow(wf));

            //var reloadedWf = wfFacade.Evaluate(c => c.GetRichWorkflow(this.wfIdent));

            //var approvingState = reloadedWf.ParallelStates.Single(s => s.Name == "Approving");

            //var subWf = wfFacade.Evaluate(c => c.GetRichWorkflow(approvingState.StartItems.Single().SubWorkflow.Identity));

            //this.approveCommand = subWf.States.Single(s => s.Name == "Approving").Tasks.Single(t => t.Name == "ApprovingTask").Commands.Single(c => c.Name == "Approve");

            //this.approveCommandCommentParameter = this.approveCommand.Parameters.Single(p => p.Name == "Comment");

            //this.approveCommandPotentialApproversParameter = this.approveCommand.Parameters.Single(p => p.Name == "PotentialApprovers");
        }

        protected sealed override SampleSystemServiceEnvironment Environment { get; } = TestServiceEnvironment.WorkflowIntegrationEnvironment;

        [TestMethod]
        public void CreatePermission_WorkflowPassed()
        {
            // Arrange
            var testUserForApproving = "ApprovingWfUser";

            var authFacade = this.GetAuthControllerEvaluator();

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
