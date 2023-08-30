using Framework.Core;

namespace Framework.Security;

public static class TransferEnumHelper
{
    public static void Check<TSecurityOperationCode>(TSecurityOperationCode securityOperationCode)
            where TSecurityOperationCode : struct, Enum
    {
        if (securityOperationCode.IsDefault() || !Enum.IsDefined(typeof(TSecurityOperationCode), securityOperationCode))
        {
            throw new ArgumentOutOfRangeException(nameof(securityOperationCode));
        }
    }

    public static TTargetSecurityOperationCode Convert<TSourceSecurityOperationCode, TTargetSecurityOperationCode>(TSourceSecurityOperationCode securityOperationCode)
            where TSourceSecurityOperationCode : struct, Enum
            where TTargetSecurityOperationCode : struct, Enum
    {
        Check(securityOperationCode);

        var result = (TTargetSecurityOperationCode)(Enum)securityOperationCode;

        Check(result);

        return result;
    }
}
