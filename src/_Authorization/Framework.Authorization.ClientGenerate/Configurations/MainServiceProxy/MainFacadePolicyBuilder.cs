using System;

using Framework.Authorization.WebApi;
using Framework.DomainDriven.DTOGenerator.TypeScript.Facade;

namespace Framework.Authorization.ClientGenerate
{
    public class MainFacadePolicyBuilder : TypeScriptMethodPolicyBuilder<AuthSLJsonController>
    {
        public MainFacadePolicyBuilder()
        {
            this.AddOperationMethods();
            this.AddBusinessRoleMethods();

            this.AddPrincipalMethods();

            this.AddUnsortedMethods();
        }

        private void AddBusinessRoleMethods()
        {
            this.Add(facade => facade.GetVisualBusinessRoles());
            this.Add(facade => facade.GetSimpleBusinessRoles());
            this.Add(facade => facade.GetSimpleBusinessRolesByRootFilter(default));

            this.Add(facade => facade.CreateBusinessRole(default));
            this.Add(facade => facade.SaveBusinessRole(default));
            this.Add(facade => facade.GetFullBusinessRolesByIdents(default));
            this.Add(facade => facade.GetFullBusinessRolesByRootFilter(default));
            this.Add(facade => facade.GetRichBusinessRole(default));
            this.Add(facade => facade.RemoveBusinessRole(default));
        }

        private void AddOperationMethods()
        {
            this.Add(facade => facade.GetSimpleOperations());
            this.Add(facade => facade.GetSimpleOperationsByRootFilter(default));

            this.Add(facade => facade.SaveOperation(default));
            this.Add(facade => facade.GetFullOperationsByIdents(default));
            this.Add(facade => facade.GetFullOperationsByRootFilter(default));
            this.Add(facade => facade.GetRichOperation(default));
        }

        private void AddPrincipalMethods()
        {
            this.Add(facade => facade.GetSimplePrincipals());
            this.Add(facade => facade.GetSimplePrincipalsByRootFilter(default));

            this.Add(facade => facade.GetSimplePrincipalByName(default));

            this.Add(facade => facade.CreatePrincipal(default));
            this.Add(facade => facade.SavePrincipal(default));
            this.Add(facade => facade.GetFullPrincipalsByIdents(default));
            this.Add(facade => facade.GetFullPrincipalsByRootFilter(default));
            this.Add(facade => facade.GetRichPrincipal(default));
            this.Add(facade => facade.RemovePrincipal(default));
        }

        private void AddUnsortedMethods()
        {
            this.Add(facade => facade.GetSecurityOperations());
            this.Add(facade => facade.GetSimpleEntityTypes());

            this.Add(facade => facade.RunAsUser(default));
            this.Add(facade => facade.FinishRunAsUser());

            this.Add(facade => facade.GetCurrentPrincipal());

            this.Add(facade => facade.GetRichPermission(default));

            this.Add(facade => facade.GetSimpleEntityTypesByRootFilter(default));
            this.Add(facade => facade.GetFullSecurityEntities(default));
            this.Add(facade => facade.GetSimpleEntityTypeByName(default));
            this.Add(facade => facade.GetRichPermissionsByDirectFilter(default));
            this.Add(facade => facade.GetFullSecurityEntitiesByIdents(default));

            this.Add(facade => facade.ChangeDelegatePermissions(default));
            this.Add(facade => facade.GetVisualPrincipalsWithoutSecurity());
            this.Add(facade => facade.GetVisualBusinessRolesByPermission(default));
        }
    }
}
