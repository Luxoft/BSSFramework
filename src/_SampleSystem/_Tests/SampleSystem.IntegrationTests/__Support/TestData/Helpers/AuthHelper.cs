using System;

using Automation.Utils;

using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven.BLL;

using SampleSystem.BLL;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

using BusinessRole = SampleSystem.IntegrationTests.__Support.Utils.BusinessRole;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers
{
    public class AuthHelper : Utils.Framework.Authorization, IRootServiceProviderContainer
    {
        public SampleSystemTestServiceEnvironment Environment { get; set; }

        public void SetUserRole(EmployeeIdentityDTO employee, params IPermissionDefinition[] permissions)
        {
            var login = this.EvaluateRead(context => context.Logics.Employee.GetById(employee.Id).Login);
            this.SetUserRole(login, permissions);
        }

        public void SetCurrentUserRole(BusinessRole businessRole)
        {
            this.SetCurrentUserRole(new SampleSystemPermission(businessRole));
        }

        public Framework.Authorization.Generated.DTO.PrincipalIdentityDTO SavePrincipal(string name, bool active, Guid? externalId = null)
        {
            return this.EvaluateWrite(context =>
            {
                var principal = new Framework.Authorization.Domain.Principal { Name = name, Active = active, ExternalId = externalId };
                context.Authorization.Logics.Principal.Save(principal);
                return principal.ToIdentityDTO();
            });
        }

        public string GetCurrentUserLogin()
        {
            return this.EvaluateRead(context => context.Authorization.CurrentPrincipalName);
        }

        public override void EvaluateWrite(Action<ISampleSystemBLLContext> action)
        {
            this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, null, action);
        }

        public TResult EvaluateWrite<TResult>(Func<ISampleSystemBLLContext, TResult> func)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, null, func);
        }

        public TResult EvaluateRead<TResult>(Func<ISampleSystemBLLContext, TResult> func)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, null, func);
        }

        public string GetEmployeeLogin(EmployeeIdentityDTO employee)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, ctx => ctx.Logics.Employee.GetById(employee.Id, true).Login);
        }

        IServiceProvider IRootServiceProviderContainer.RootServiceProvider => this.Environment.RootServiceProvider;
    }
}
