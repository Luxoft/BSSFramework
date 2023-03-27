using System.Collections;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL.Security;
using Framework.Security;

using JetBrains.Annotations;

namespace Framework.Authorization.BLL;

public static class OperationBLLExtensions
{
    public static IEnumerable<TSecurityOperationCode> GetAvailableOperationCodes<TSecurityOperationCode>(this IOperationBLL bll)
            where TSecurityOperationCode : struct, Enum
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));

        return bll.GetAvailableOperations().ToSecurityOperationCodes<TSecurityOperationCode>();
    }

    public static IEnumerable<string> GetAvailableOperationCodes(this IOperationBLL bll)
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));

        var operations = bll.GetAvailableOperations().ToList();

        return bll.Context.ExternalSource
                  .SecurityOperationCodeType
                  .GetSecurityOperationTypes()
                  .SelectMany(operations.ToSecurityOperationCodes)
                  .Select(securityOperationCode => securityOperationCode.ToString())
                  .Distinct()
                  .OrderBy(v => v);
    }

    private static IEnumerable<TSecurityOperationCode> ToSecurityOperationCodes<TSecurityOperationCode>([NotNull] this IEnumerable<Operation> operations)
            where TSecurityOperationCode : struct, Enum
    {
        if (operations == null) throw new ArgumentNullException(nameof(operations));

        return operations.Select(operation => operation.Id.ToSecurityOperation<TSecurityOperationCode>()).CollectMaybe();
    }

    private static IEnumerable<Enum> ToSecurityOperationCodes([NotNull] this IEnumerable<Operation> operations, [NotNull] Type securityOperationType)
    {
        if (operations == null) throw new ArgumentNullException(nameof(operations));
        if (securityOperationType == null) throw new ArgumentNullException(nameof(securityOperationType));

        return from Enum securityOperationCode in new Func<IEnumerable<Operation>, IEnumerable<SecurityOperationCode>>(ToSecurityOperationCodes<SecurityOperationCode>)
                                                  .CreateGenericMethod(securityOperationType)
                                                  .Invoke<IEnumerable>(null, new object[] { operations })

               select securityOperationCode;
    }
}
