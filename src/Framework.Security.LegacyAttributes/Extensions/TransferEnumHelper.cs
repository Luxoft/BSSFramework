using Framework.SecuritySystem;

namespace Framework.Security;

public static class TransferEnumHelper
{
    public static void Check(string securityOperationName, Type securityType)
    {
        Convert(securityOperationName, securityType);
    }

    public static SecurityOperation Convert(string securityOperationName, Type securityType)
    {
        var securityOperation = (SecurityOperation)securityType.GetProperty(securityOperationName)?.GetValue(null);

        if (securityOperation is null || securityOperation is DisabledSecurityOperation)
        {
            throw new ArgumentOutOfRangeException(nameof(securityOperationName));
        }

        return securityOperation;
    }
}
