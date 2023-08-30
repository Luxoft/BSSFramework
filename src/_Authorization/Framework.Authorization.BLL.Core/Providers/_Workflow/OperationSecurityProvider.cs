using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.Authorization.BLL
{
    public class OperationSecurityProvider : SecurityProvider<Operation>
    {
        private readonly Lazy<List<Operation>> lazyOperations;

        public OperationSecurityProvider(IAuthorizationBLLContext context)
        {
            this.Context = context;
            this.lazyOperations = LazyHelper.Create(() =>

                                                        this.Context.Logics.Permission.GetAvailablePermissionsQueryable()
                                                            .SelectMany(permission => permission.Role.BusinessRoleOperationLinks)
                                                            .Select(link => link.Operation)
                                                            .Distinct()
                                                            .ToList());
        }

        public IAuthorizationBLLContext Context { get; }

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

            return this.Context.Logics.Permission.GetAvailablePermissionsQueryable(new AvailablePermissionFilter(this.Context.DateTimeService, null))
                                                 .Where(permission => permission.Role.BusinessRoleOperationLinks.Any(link => link.Operation == operation))
                                                 .Select(permission => permission.Principal.Name)
                                                 .Distinct()
                                                 .ToUnboundedList();
        }
    }
}
