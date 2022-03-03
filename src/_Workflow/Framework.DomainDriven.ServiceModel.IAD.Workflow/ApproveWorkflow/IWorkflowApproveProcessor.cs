using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.Authorization.Domain;

using JetBrains.Annotations;

namespace Framework.Authorization.BLL
{
    public interface IWorkflowApproveProcessor
    {
        IEnumerable<string> GetPermissionFromPrincipalPath([NotNull] Permission permission);

        IEnumerable<string> GetPermissionDefaultPath([NotNull] Permission permission);

        IEnumerable<string> GetOperationDefaultPath([NotNull] Operation operation);

        Expression<Func<Principal, IEnumerable<Permission>>> GetPermissionsByPrincipal();

        IEnumerable<ApproveOperationWorkflowObject> GetApproveOperationStartupObjects([NotNull] Permission permission);

        bool CanAutoApprove([NotNull] Permission permission, [NotNull] Operation approveOperation);

        void ExecuteApproveCommand([NotNull] Permission permission, [NotNull] Operation approveOperation, [NotNull] ApproveCommand command);

        bool IsTerminate([NotNull] Permission permission);

        StartupPermissionWorkflowObject Start([NotNull] Permission permission);
    }
}
