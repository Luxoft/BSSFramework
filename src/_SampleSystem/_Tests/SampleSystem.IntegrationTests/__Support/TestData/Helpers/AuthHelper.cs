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
        public AuthHelper(IServiceProvider rootServiceProvider)
            : base(rootServiceProvider)
        {
        }

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

        public string GetEmployeeLogin(EmployeeIdentityDTO employee)
        {
            return this.GetContextEvaluator().Evaluate(DBSessionMode.Read, ctx => ctx.Logics.Employee.GetById(employee.Id, true).Login);
        }
    }
}
