using System;
using System.Linq;

using Automation.Utils;

using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven.BLL;

using WorkflowSampleSystem.BLL;
using WorkflowSampleSystem.Generated.DTO;
using WorkflowSampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using WorkflowSampleSystem.ServiceEnvironment;

using BusinessRole = WorkflowSampleSystem.IntegrationTests.__Support.Utils.BusinessRole;

namespace WorkflowSampleSystem.IntegrationTests.__Support.TestData.Helpers
{
    public class AuthHelper : Utils.Framework.Authorization, IControllerEvaluatorContainer
    {
        public AuthHelper()
        {
        }

        public AuthHelper(WorkflowSampleSystemServiceEnvironment environment)
        {
            this.Environment = environment;
        }

        public WorkflowSampleSystemServiceEnvironment Environment { get; set; }

        public void SetUserRole(EmployeeIdentityDTO employee, params IPermissionDefinition[] permissions)
        {
            var login = this.EvaluateRead(context => context.Logics.Employee.GetById(employee.Id).Login);
            this.SetUserRole(login, permissions);
        }

        public void SetCurrentUserRole(BusinessRole businessRole)
        {
            this.SetCurrentUserRole(new WorkflowSampleSystemPermission(businessRole));
        }

        public void LoginAs(EmployeeIdentityDTO employee)
        {
            this.LoginAs(this.GetEmployeeLogin(employee));
        }

        public void RunCommandAs(EmployeeIdentityDTO employee, Action command)
        {
            this.LoginAs(employee);
            command.Invoke();
            this.FinishRunAsUser();
        }

        public void LoginAs(string principalName = null)
        {
            IntegrationTestsUserAuthenticationService.Instance.CustomUserName = principalName;
        }

        public new void FinishRunAsUser()
        {
            this.LoginAs();
        }

        public string GetCurrentUserLogin()
        {
            return this.EvaluateRead(context => context.Authorization.CurrentPrincipalName);
        }

        public override void EvaluateWrite(Action<IWorkflowSampleSystemBLLContext> action)
        {
            this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, null, action);
        }

        public TResult EvaluateRead<TResult>(Func<IWorkflowSampleSystemBLLContext, TResult> func)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, null, func);
        }

        public string GetEmployeeLogin(EmployeeIdentityDTO employee)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, ctx => ctx.Logics.Employee.GetById(employee.Id, true).Login);
        }

        public PrincipalFullDTO GetPrincipalByName(string login)
        {
            return this.EvaluateRead(ctx => ctx.Authorization.Logics.Principal.GetObjectsBy(x => x.Name.Equals(login)).Select(x => x?.ToFullDTO(new AuthorizationServerPrimitiveDTOMappingService(ctx.Authorization))).FirstOrDefault());
        }


        IServiceProvider IControllerEvaluatorContainer.RootServiceProvider => this.Environment.RootServiceProvider;
    }
}
