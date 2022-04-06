using System;

using Automation.Utils;

using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven.BLL;
using SampleSystem.BLL;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.Utils.Framework;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore;
using BusinessRole = SampleSystem.IntegrationTests.__Support.Utils.BusinessRole;
using PrincipalFullDTO = Framework.Authorization.Generated.DTO.PrincipalFullDTO;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers
{
    public class AuthHelper : Utils.Framework.Authorization, IControllerEvaluatorContainer
    {
        public AuthHelper()
        {
        }

        public AuthHelper(SampleSystemServiceEnvironment environment)
        {
            this.Environment = environment;
        }

        public SampleSystemServiceEnvironment Environment { get; set; }

        public void SetUserRole(EmployeeIdentityDTO employee, params IPermissionDefinition[] permissions)
        {
            var login = this.EvaluateRead(context => context.Logics.Employee.GetById(employee.Id).Login);
            this.SetUserRole(login, permissions);
        }

        public void SetCurrentUserRole(BusinessRole businessRole)
        {
            this.SetCurrentUserRole(new SampleSystemPermission(businessRole));
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

        public override void EvaluateWrite(Action<ISampleSystemBLLContext> action)
        {
            this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, null, action);
        }

        public TResult EvaluateRead<TResult>(Func<ISampleSystemBLLContext, TResult> func)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, null, func);
        }

        public string GetEmployeeLogin(EmployeeIdentityDTO employee)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, ctx => ctx.Logics.Employee.GetById(employee.Id, true).Login);
        }

        public PrincipalFullDTO GetPrincipalByName(string login)
        {
            return this.EvaluateRead(ctx => ctx.Authorization.Logics.Principal.GetListBy(x => x.Name.Equals(login))
                                               .Select(x => x.ToFullDTO(new AuthorizationServerPrimitiveDTOMappingService(ctx.Authorization)))
                                               .FirstOrDefault());
        }


        IServiceProvider IControllerEvaluatorContainer.RootServiceProvider => this.Environment.RootServiceProvider;
    }
}
