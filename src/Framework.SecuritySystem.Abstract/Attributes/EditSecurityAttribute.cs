using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem;

[AttributeUsage(AttributeTargets.Parameter)]
public class EditSecurityAttribute() : FromKeyedServicesAttribute(nameof(SecurityRule.Edit));
