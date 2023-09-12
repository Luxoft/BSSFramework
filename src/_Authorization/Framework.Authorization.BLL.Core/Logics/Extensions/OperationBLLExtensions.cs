//using System.Collections;

//using Framework.Authorization.Domain;
//using Framework.Core;
//using Framework.DomainDriven.BLL.Security;
//using Framework.Security;

//namespace Framework.Authorization.BLL;

//public static class OperationBLLExtensions
//{
//    public static IEnumerable GetAvailableOperationCodes(this IOperationBLL bll)
//            where TSecurityOperationCode : struct, Enum
//    {
//        if (bll == null) throw new ArgumentNullException(nameof(bll));

//        return bll.GetAvailableOperations().ToSecurityOperationCodes();
//    }

//    public static IEnumerable<string> GetAvailableOperationCodes(this IOperationBLL bll)
//    {
//        if (bll == null) throw new ArgumentNullException(nameof(bll));

//        var operations = bll.GetAvailableOperations().ToList();

//        return bll.Context.ExternalSource
//                  .SecurityOperationCodeType
//                  .GetSecurityOperationTypes()
//                  .SelectMany(operations.ToSecurityOperationCodes)
//                  .Select(securityOperationCode => securityOperationCode.ToString())
//                  .Distinct()
//                  .OrderBy(v => v);
//    }

//    private static IEnumerable ToSecurityOperationCodes(this IEnumerable<Operation> operations)
//            where TSecurityOperationCode : struct, Enum
//    {
//        if (operations == null) throw new ArgumentNullException(nameof(operations));

//        return operations.Select(operation => operation.Id.ToSecurityOperation()).CollectMaybe();
//    }

//    private static IEnumerable<Enum> ToSecurityOperationCodes(this IEnumerable<Operation> operations, Type securityOperationType)
//    {
//        if (operations == null) throw new ArgumentNullException(nameof(operations));
//        if (securityOperationType == null) throw new ArgumentNullException(nameof(securityOperationType));

//        return from Enum securityOperationCode in new Func<IEnumerable<Operation>, IEnumerable<SecurityOperationCode>>(ToSecurityOperationCodes<SecurityOperationCode>)
//                                                  .CreateGenericMethod(securityOperationType)
//                                                  .Invoke<IEnumerable>(null, new object[] { operations })

//               select securityOperationCode;
//    }
//}
