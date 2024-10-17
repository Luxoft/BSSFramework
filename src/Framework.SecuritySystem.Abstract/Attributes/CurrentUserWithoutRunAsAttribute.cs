using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem;

public class CurrentUserWithoutRunAsAttribute() : FromKeyedServicesAttribute(nameof(SecurityRuleCredential.CurrentUserWithoutRunAsCredential));
