using System;

using Automation.Utils;

using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven.BLL;
using WorkflowSampleSystem.BLL;
using WorkflowSampleSystem.Generated.DTO;
using WorkflowSampleSystem.IntegrationTests.__Support.Utils.Framework;
using WorkflowSampleSystem.ServiceEnvironment;
using WorkflowSampleSystem.WebApiCore;
using BusinessRole = WorkflowSampleSystem.IntegrationTests.__Support.Utils.BusinessRole;

namespace WorkflowSampleSystem.IntegrationTests.__Support.TestData.Helpers
{
    public class AuthHelper : Utils.Framework.Authorization
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
            var principalName = this.EvaluateRead(context => context.Logics.Employee.GetById(employee.Id).Login);

            this.SetUserRole(principalName, permissions);
        }

        public void SetCurrentUserRole(params BusinessRole[] businessRoles)
        {
            this.SetCurrentUserRole(businessRoles.ToArray(businessRole => new WorkflowSampleSystemPermission(businessRole)));
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

        public string GetCurrentUserLogin(IWorkflowSampleSystemBLLContext context)
        {
            return context.Authorization.CurrentPrincipalName;
        }

        public override void EvaluateWrite(Action<IWorkflowSampleSystemBLLContext> action)
        {
            this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, null, action);
        }

        public TResult EvaluateWrite<TResult>(Func<IWorkflowSampleSystemBLLContext, TResult> func)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, null, func);
        }

        public TResult EvaluateRead<TResult>(Func<IWorkflowSampleSystemBLLContext, TResult> func)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, null, func);
        }
    }
}
