using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;

using JetBrains.Annotations;

namespace Framework.Authorization.BLL
{
    public class WorkflowApproveProcessor : BLLContextContainer<IAuthorizationBLLContext>, IWorkflowApproveProcessor
    {
        public WorkflowApproveProcessor([NotNull] IAuthorizationBLLContext context)
            : base(context)
        {
        }


        public IEnumerable<string> GetPermissionFromPrincipalPath(Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            yield return this.GetWorkflowPrincipalElementName(permission.Principal);
            yield return this.GetWorkflowPermissionElementName(permission);
        }

        public IEnumerable<string> GetPermissionDefaultPath(Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            yield return this.GetWorkflowPermissionElementName(permission);
        }

        public IEnumerable<string> GetOperationDefaultPath(Operation operation)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            yield return this.GetWorkflowOperationElementName(operation);
        }

        public Expression<Func<Principal, IEnumerable<Permission>>> GetPermissionsByPrincipal()
        {
            return principal => principal.Permissions.Where(p => p.Status == PermissionStatus.Approving);
        }

        public IEnumerable<ApproveOperationWorkflowObject> GetApproveOperationStartupObjects(Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            return from link in permission.Role.BusinessRoleOperationLinks

                   let operation = link.Operation

                   where operation.ApproveOperation != null

                   group operation by operation.ApproveOperation into g

                   select new ApproveOperationWorkflowObject
                   {
                       DomainObject = g.Key,
                       Name = this.GetWorkflowOperationElementName(g.Key),
                       Description = $"Approving operations: {g.OrderBy(op => op.Name).Join(", ", op => op.Name)}"
                   };
        }

        public virtual bool CanAutoApprove(Permission permission, Operation approveOperation)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));
            if (approveOperation == null) throw new ArgumentNullException(nameof(approveOperation));

            var createdByPrincipal = permission.CreatedBy == this.Context.CurrentPrincipalName
                                   ? this.Context.CurrentPrincipal
                                   : this.Context.Logics.Principal.GetByName(permission.CreatedBy);

            var autoApprove = permission.DelegatedFrom != null && createdByPrincipal.GetOperations(this.Context.DateTimeService.Now).Contains(approveOperation);

            return autoApprove;
        }

        public void ExecuteApproveCommand(Permission permission, Operation approveOperation, ApproveCommand command)
        {

        }

        public bool IsTerminate(Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            return permission.Status == PermissionStatus.Rejected;
        }

        public StartupPermissionWorkflowObject Start(Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            return new StartupPermissionWorkflowObject
            {
                DomainObject = permission,
                Name = $"Approving \"{permission.Role.Name}\" Permission Role \"{permission.Principal.Name}\" with duration \"{permission.Period}\""
            };
        }



        private string GetWorkflowPrincipalElementName(Principal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            return $"Principal: {principal.Name}";
        }

        private string GetWorkflowPermissionElementName(Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            return $"Permission: Role \"{permission.Role.Name}\" with duration \"{permission.Period}\" ";
        }

        private string GetWorkflowOperationElementName(Operation operation)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            return $"Approve by operation: {operation.Name}";
        }
    }
}
