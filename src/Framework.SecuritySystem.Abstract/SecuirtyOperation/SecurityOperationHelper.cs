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
}
