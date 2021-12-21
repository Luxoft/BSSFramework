using System;

using Automation.Utils;

using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven.BLL;
using SampleSystem.BLL;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.Utils.Framework;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore;
using BusinessRole = SampleSystem.IntegrationTests.__Support.Utils.BusinessRole;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers
{
    public class AuthHelper : Utils.Framework.Authorization
    {
        public AuthHelper()
        {
        }

        public AuthHelper(CoreSampleSystemServiceEnvironment environment)
        {
            this.Environment = environment;
        }

        public CoreSampleSystemServiceEnvironment Environment { get; set; }

        public void SetUserRole(EmployeeIdentityDTO employee, params IPermissionDefinition[] permissions)
        {
            var principalName = this.EvaluateRead(context => context.Logics.Employee.GetById(employee.Id).Login);

            this.SetUserRole(principalName, permissions);
        }

        public void SetCurrentUserRole(params BusinessRole[] businessRoles)
        {
            this.SetCurrentUserRole(businessRoles.ToArray(businessRole => new SampleSystemPermission(businessRole)));
        }

        public void LoginAs(EmployeeIdentityDTO employee)
        {
            this.EvaluateWrite(context =>
                {
                this.PrincipalName = context.Logics.Employee.GetById(employee.Id).Login;
                });

            this.LoginAs(this.PrincipalName);
        }

        public Framework.Authorization.Generated.DTO.PrincipalIdentityDTO SavePrincipal(string name, bool active, Guid? externalId = null)
        {
            return this.EvaluateWrite(context =>
            {
                var principal = new Principal { Name = name, Active = active, ExternalId = externalId };
                context.Authorization.Logics.Principal.Save(principal);
                return principal.ToIdentityDTO();
            });
        }

        public void LoginAs(string principalName = null, bool asAdmin = true)
        {
            this.PrincipalName = principalName;

            if (asAdmin)
            {
                this.SetCurrentUserRole(BusinessRole.Administrator, BusinessRole.SystemIntegration);
            }

            if (principalName != null)
            {
                this.EvaluateWrite(context =>
                {
                    context.Authorization.RunAsManager.StartRunAsUser(principalName);
                });
            }
        }

        public string GetCurrentUserLogin()
        {
            return this.EvaluateRead(
                context => context.Authorization.CurrentPrincipalName);
        }

        public string GetCurrentUserLogin(ISampleSystemBLLContext context)
        {
            return context.Authorization.CurrentPrincipalName;
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
    }
}
