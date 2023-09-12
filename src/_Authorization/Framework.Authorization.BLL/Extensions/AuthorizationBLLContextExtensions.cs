//using Framework.Authorization.Domain;
//using Framework.Core;
//using Framework.Security;

//using Serilog;

//namespace Framework.Authorization.BLL;

//public static class AuthorizationBLLContextExtensions
//{
//    public static void InitSecurityOperations(this IAuthorizationBLLContext context, InitSecurityOperationMode initSecurityOperationMode = InitSecurityOperationMode.Add)
//    {
//        if (context == null) throw new ArgumentNullException(nameof(context));

//        var dbOperations = context.Logics.Operation.GetFullList();

//        var operationDict = Framework.DomainDriven.BLL.Security.SecurityOperationCodeExtensions.GetDictionary(context.ExternalSource.SecurityOperationCodeType, true, true);

//        var mergeResult = dbOperations.GetMergeResult(operationDict, operation => operation.Id, pair => pair.Value.Guid);

//        var addingOperationPairs = mergeResult.AddingItems.Select(pair => new
//                                                                          {
//                                                                                  Attribute = pair.Value,
//                                                                                  Operation = new Operation
//                                                                                      {
//                                                                                              Name = pair.Key.ToString(),
//                                                                                              Description = pair.Value.Description,
//                                                                                      },
//                                                                                  Code = pair.Key
//                                                                          }).Where(pair => initSecurityOperationMode.HasFlag(InitSecurityOperationMode.Remove) || !dbOperations.Select(dbOperation => dbOperation.Name).Contains(pair.Operation.Name, StringComparer.CurrentCultureIgnoreCase)).ToArray();

//        if (initSecurityOperationMode.HasFlag(InitSecurityOperationMode.Remove))
//        {
//            foreach (var removingItem in mergeResult.RemovingItems)
//            {
//                Log.Verbose("Remove Operation: {RemovingItemName} {RemovingItemId}", removingItem.Name, removingItem.Id);
//                context.Logics.Operation.Remove(removingItem);
//            }
//        }

//        if (initSecurityOperationMode.HasFlag(InitSecurityOperationMode.Add) && addingOperationPairs.Any())
//        {
//            foreach (var addingOperationPair in addingOperationPairs)
//            {
//                var approveAttr = addingOperationPair.Code.ToFieldInfo().GetCustomAttribute<ApproveOperationAttribute>();

//                if (approveAttr != null)
//                {
//                    addingOperationPair.Operation.ApproveOperation = addingOperationPairs.SingleOrDefault(pair => approveAttr.Operation.Equals(pair.Code)).Maybe(pair => pair.Operation)
//                                                                     ?? context.Logics.Operation.GetById(operationDict[approveAttr.Operation].Guid, true);
//                }

//                Log.Verbose("Add Operation: {OperationName} {AttributeGuid}", addingOperationPair.Operation.Name, addingOperationPair.Attribute.Guid);
//                context.Logics.Operation.Insert(addingOperationPair.Operation, addingOperationPair.Attribute.Guid);
//            }

//            var admRole = context.Logics.BusinessRole.GetOrCreateAdminRole();

//            addingOperationPairs.Where(pair => pair.Attribute.AdminHasAccess).Foreach(pair => new BusinessRoleOperationLink(admRole)
//                                                                                          {
//                                                                                                  Operation = pair.Operation
//                                                                                          });

//            context.Logics.BusinessRole.Save(admRole);
//        }
//    }

//    public static void InitApproveOperations(this IAuthorizationBLLContext context)
//    {
//        if (context == null) throw new ArgumentNullException(nameof(context));

//        var bll = context.Logics.Operation;

//        var dbOperations = bll.GetFullList();

//        var operationDict = Framework.DomainDriven.BLL.Security.SecurityOperationCodeExtensions.GetDictionary(context.ExternalSource.SecurityOperationCodeType, true, true);

//        var pairRequest = from operation in dbOperations

//                          join pair in operationDict on operation.Id equals pair.Value.Guid

//                          let field = context.ExternalSource.SecurityOperationCodeType.GetField(pair.Key.ToString())

//                          let approveOperationRequest = from approveAtttribute in field.GetCustomAttribute<ApproveOperationAttribute>().ToMaybe()

//                                                        let approveOperationCode = approveAtttribute.Operation

//                                                        let approvePair = operationDict[approveOperationCode]

//                                                        let dbOperation = dbOperations.SingleOrDefault(dbOperation => dbOperation.Id == approvePair.Guid)

//                                                        where dbOperation != null

//                                                        select dbOperation

//                          let approveOperation = approveOperationRequest.GetValueOrDefault()

//                          where operation.ApproveOperation != approveOperation

//                          select new
//                                 {
//                                         Operation = operation,
//                                         ApproveOperation = approveOperation
//                                 };

//        var pairs = pairRequest.ToList();

//        foreach (var pair in pairs)
//        {
//            pair.Operation.ApproveOperation = pair.ApproveOperation;

//            bll.Save(pair.Operation);
//        }
//    }
//}
