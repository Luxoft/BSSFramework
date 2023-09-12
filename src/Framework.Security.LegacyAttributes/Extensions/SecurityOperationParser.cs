using Framework.SecuritySystem;

namespace Framework.Security;

public static class SecurityOperationParser
{
    public static SecurityOperation ConvertToOperation(string securityOperationName, Type securityOperationType)
    {
        var securityOperation = (SecurityOperation)securityOperationType.GetProperty(securityOperationName)?.GetValue(null);

        if (securityOperation is null || securityOperation is DisabledSecurityOperation)
        {
            throw new ArgumentOutOfRangeException(nameof(securityOperationName));
        }

        return securityOperation;
    }
}
