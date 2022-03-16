using System;
using System.Linq;

using Automation.Utils;

using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven.BLL;

using AttachmentsSampleSystem.BLL;
using AttachmentsSampleSystem.Generated.DTO;
using AttachmentsSampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using AttachmentsSampleSystem.ServiceEnvironment;

using BusinessRole = AttachmentsSampleSystem.IntegrationTests.__Support.Utils.BusinessRole;

namespace AttachmentsSampleSystem.IntegrationTests.__Support.TestData.Helpers
{
    public class AuthHelper : Utils.Framework.Authorization, IControllerEvaluatorContainer
    {
        public AuthHelper()
        {
        }

        public AuthHelper(AttachmentsSampleSystemServiceEnvironment environment)
        {
            this.Environment = environment;
        }

        public AttachmentsSampleSystemServiceEnvironment Environment { get; set; }

        public void SetUserRole(EmployeeIdentityDTO employee, params IPermissionDefinition[] permissions)
        {
            var login = this.EvaluateRead(context => context.Logics.Employee.GetById(employee.Id).Login);
            this.SetUserRole(login, permissions);
        }

        public void SetCurrentUserRole(BusinessRole businessRole)
        {
            this.SetCurrentUserRole(new AttachmentsSampleSystemPermission(businessRole));
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

        public override void EvaluateWrite(Action<IAttachmentsSampleSystemBLLContext> action)
        {
            this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, null, action);
        }

        public TResult EvaluateRead<TResult>(Func<IAttachmentsSampleSystemBLLContext, TResult> func)
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
