using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Authorization.Domain;
using Framework.Persistent;

namespace Framework.Authorization.BLL;

public partial class OperationBLL
{
    public IEnumerable<Operation> GetAvailableOperations()
    {
        return this.Context.Logics.Permission.GetAvailablePermissionsQueryable()
                   .Select(permission => permission.Role)
                   .SelectMany(businessRole => businessRole.BusinessRoleOperationLinks)
                   .Select(link => link.Operation)
                   .Distinct()
                   .ToList();
    }

    public override void Remove(Operation operation)
    {
        if (operation == null)
        {
            throw new ArgumentNullException(nameof(operation));
        }

        foreach (var link in operation.Links)
        {
            link.BusinessRole.RemoveDetail(link);
        }

        base.Remove(operation);
    }
}
