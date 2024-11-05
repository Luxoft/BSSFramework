using Framework.Core;

namespace Framework.SecuritySystem.Services;

public class SecurityRuleTypeResolver : ITypeResolver<SecurityRule>
{
    public Type Resolve(SecurityRule securityRule) => securityRule.GetType();

    public IEnumerable<Type> GetTypes() =>
        typeof(SecurityRule).Assembly.GetTypes().Where(t => !t.IsAbstract && typeof(SecurityRule).IsAssignableFrom(t));
}
