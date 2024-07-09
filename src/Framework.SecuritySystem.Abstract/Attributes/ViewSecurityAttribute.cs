#nullable enable
using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem;

[AttributeUsage(AttributeTargets.Parameter)]
public class ViewSecurityAttribute() : FromKeyedServicesAttribute(nameof(SecurityRule.View));
