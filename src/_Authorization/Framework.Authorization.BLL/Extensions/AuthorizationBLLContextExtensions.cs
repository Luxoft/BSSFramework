using System.Linq;
using System.Reflection;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Security;
using Framework.SecuritySystem;

using Serilog;

namespace Framework.Authorization.BLL;

public static class AuthorizationBLLContextExtensions
{
    public static void InitSecurityOperations(this IAuthorizationBLLContext context, Type[] securityOperationTypes, InitSecurityOperationMode initSecurityOperationMode = InitSecurityOperationMode.Add)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var dbOperations = context.Logics.Operation.GetFullList();

        var securityOperations = securityOperationTypes.SelectMany(SecurityOperationHelper.GetSecurityOperations).Distinct()
                                                       .OfType<ISecurityOperation<Guid>>();

        var mergeResult = dbOperations.GetMergeResult(securityOperations, operation => operation.Id, operation => operation.Id);

        var addingOperationPairs = mergeResult.AddingItems
                                              .Select(
                                                  securityOperation => new
                                                                       {
                                                                           AuthOperation = new Operation
                                                                                           {
                                                                                               Name = securityOperation.Name,
                                                                                               Description = securityOperation.Description,
                                                                                           },
                                                                           SecurityOperation = securityOperation
                                                                       })
                                              .Where(
                                                  pair => initSecurityOperationMode.HasFlag(InitSecurityOperationMode.Remove)
                                                          || !dbOperations.Select(dbOperation => dbOperation.Name).Contains(
                                                              pair.AuthOperation.Name,
                                                              StringComparer.CurrentCultureIgnoreCase))
                                              .ToArray();

        if (initSecurityOperationMode.HasFlag(InitSecurityOperationMode.Remove))
        {
            foreach (var removingItem in mergeResult.RemovingItems)
            {
                Log.Verbose("Remove Operation: {RemovingItemName} {RemovingItemId}", removingItem.Name, removingItem.Id);
                context.Logics.Operation.Remove(removingItem);
            }
        }

        if (initSecurityOperationMode.HasFlag(InitSecurityOperationMode.Add) && addingOperationPairs.Any())
        {
            foreach (var addingOperationPair in addingOperationPairs)
            {
                if (addingOperationPair.SecurityOperation.ApproveOperation is ISecurityOperation<Guid> approveOperation)
                {
                    addingOperationPair.AuthOperation.ApproveOperation = addingOperationPairs.SingleOrDefault(pair => approveOperation.Id == pair.SecurityOperation.Id).Maybe(pair => pair.AuthOperation)
                                                                     ?? dbOperations.Single(dbOperation => dbOperation.Id == approveOperation.Id);
                }

                Log.Verbose("Add Operation: {OperationName} {AttributeGuid}", addingOperationPair.SecurityOperation.Name, addingOperationPair.SecurityOperation.Id);
                context.Logics.Operation.Insert(addingOperationPair.AuthOperation, addingOperationPair.SecurityOperation.Id);
            }

            var admRole = context.Logics.BusinessRole.GetOrCreateAdminRole();

            addingOperationPairs.Where(pair => pair.SecurityOperation.AdminHasAccess).Foreach(pair => new BusinessRoleOperationLink(admRole)
            {
                Operation = pair.AuthOperation
            });

            context.Logics.BusinessRole.Save(admRole);
        }
    }
}
