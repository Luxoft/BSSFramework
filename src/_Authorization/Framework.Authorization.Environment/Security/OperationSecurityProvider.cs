using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.Authorization.Environment
{
    public class OperationSecurityProvider : SecurityProvider<Operation>
    {
        private readonly IAvailablePermissionSource availablePermissionSource;

        private readonly Lazy<List<Operation>> lazyOperations;

        public OperationSecurityProvider(IAvailablePermissionSource availablePermissionSource)
        {
            this.availablePermissionSource = availablePermissionSource;

            this.lazyOperations = LazyHelper.Create(
                () =>

                    this.availablePermissionSource.GetAvailablePermissionsQueryable()
                        .SelectMany(permission => permission.Role.BusinessRoleOperationLinks)
                        .Select(link => link.Operation)
                        .Distinct()
                        .ToList());
        }

        public override Expression<Func<Operation, bool>> SecurityFilter
        {
            get
            {
                var operations = this.lazyOperations.Value;

                return operation => operations.Contains(operation);
            }
        }

        public override UnboundedList<string> GetAccessors(Operation operation)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            return this.availablePermissionSource
                       .GetAvailablePermissionsQueryable(securityRoleIdents: operation.Id, applyCurrentUser: false)
                       .Select(permission => permission.Principal.Name)
                       .Distinct()
                       .ToUnboundedList();
        }
    }
}
