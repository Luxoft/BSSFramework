using System.Reflection;

using Framework.SecuritySystem;

namespace Framework.Security;

public static class SecurityOperationHelper
{
    public static IEnumerable<SecurityOperation> GetSecurityOperations(Type securityOperationType)
    {
        return from prop in securityOperationType.GetProperties(BindingFlags.Static | BindingFlags.Public)

               where typeof(SecurityOperation).IsAssignableFrom(prop.PropertyType)

               select (SecurityOperation)prop.GetValue(null);
    }

    public static SecurityOperation Parse(Type securityOperationType, string name)
    {
        var securityOperation = GetSecurityOperations(securityOperationType).Single(operation => operation.Name == name);

        if (securityOperation is null || securityOperation is DisabledSecurityOperation)
        {
            throw new ArgumentOutOfRangeException(nameof(name));
        }

        return securityOperation;
    }

    public static SecurityOperation Parse(Type securityOperationType, Enum value)
    {
        return Parse(securityOperationType, value.ToString());
    }
}
