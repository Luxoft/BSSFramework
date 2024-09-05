using Framework.Authorization.Domain;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public static class PermissionExtensions
{
    public static Dictionary<Type, List<Guid>> ToDictionary(
        this Permission permission,
        IRealTypeResolver realTypeResolver,
        ISecurityContextSource securityContextSource,
        IEnumerable<Type> securityContextTypes)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        var request = from restriction in permission.Restrictions

                      join securityContextType in securityContextTypes on restriction.SecurityContextType.Id equals securityContextSource
                          .GetSecurityContextInfo(realTypeResolver.Resolve(securityContextType)).Id

                      group restriction.SecurityContextId by securityContextType;

        return request.ToDictionary(g => g.Key, g => g.ToList());
    }
}
