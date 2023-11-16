using System.Reflection;

using Framework.Core;

namespace Framework.Authorization.SecuritySystem;

public class SecurityRoleSource : ISecurityRoleSource
{
    private readonly Lazy<List<SecurityRole>> lazySecurityRoleTypeInfoList;

    public SecurityRoleSource(IEnumerable<SecurityRoleTypeInfo> securityRoleTypeInfoList)
    {
        this.lazySecurityRoleTypeInfoList = LazyHelper.Create(
            () =>
            {
                var request = from securityRoleTypeInfo in securityRoleTypeInfoList

                              from property in securityRoleTypeInfo.SecurityRoleType.GetProperties(
                                  BindingFlags.Public | BindingFlags.Static)

                              where typeof(SecurityRole).IsAssignableFrom(property.PropertyType)

                              select (SecurityRole)property.GetValue(null);

                return request.ToList();
            });
    }

    public IReadOnlyList<SecurityRole> SecurityRoles => this.lazySecurityRoleTypeInfoList.Value;
}
