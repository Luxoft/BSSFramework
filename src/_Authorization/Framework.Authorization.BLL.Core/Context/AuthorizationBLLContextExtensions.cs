using System;

using Framework.Authorization.Domain;
using Framework.SecuritySystem;

namespace Framework.Authorization.BLL
{
    public static class AuthorizationBLLContextExtensions
    {
        public static ISecurityProvider<Principal> GetPrincipalSecurityProvider(this IAuthorizationBLLContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return context.GetPrincipalSecurityProvider<Principal>(principal => principal);
        }

        public static ISecurityProvider<BusinessRole> GetBusinessRoleSecurityProvider(this IAuthorizationBLLContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return context.GetBusinessRoleSecurityProvider<BusinessRole>(businessRole => businessRole);
        }
    }
}
