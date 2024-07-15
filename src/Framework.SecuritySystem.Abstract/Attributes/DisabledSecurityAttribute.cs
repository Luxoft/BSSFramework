using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem;

[AttributeUsage(AttributeTargets.Parameter)]
public class DisabledSecurityAttribute() : FromKeyedServicesAttribute(nameof(SecurityRule.Disabled));
