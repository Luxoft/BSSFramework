using System;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Authorization.Domain
{
    public class PrincipalRootFilterModel : DomainObjectRootFilterModel<Principal>
    {
        public Operation Operation { get; set; }

        public BusinessRole BusinessRole { get; set; }

        public override Expression<Func<Principal, bool>> ToFilterExpression()
        {
            var businessRole = this.BusinessRole;

            var operation = this.Operation;

            return principal => (businessRole == null || principal.Permissions.Any(permission => permission.Role == businessRole))
                             && (operation == null || principal.Permissions.Any(permission => permission.Role.BusinessRoleOperationLinks.Any(link => link.Operation == operation)));
        }
    }
}
