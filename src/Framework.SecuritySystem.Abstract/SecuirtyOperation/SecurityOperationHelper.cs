using System.Reflection;

namespace Framework.SecuritySystem;

public static class SecurityRoleHelper
{
    public static IEnumerable<SecurityRole> GetSecurityRoles(Type securityRoleType, Func<string, bool> filter)
    {
        return from prop in securityRoleType.GetProperties(BindingFlags.Static | BindingFlags.Public)

               where filter(prop.Name)

               where typeof(SecurityRole).IsAssignableFrom(prop.PropertyType)

               select (SecurityRole)prop.GetValue(null);
    }
}
