using Framework.SecuritySystem.ProviderFactories;

namespace Framework.SecuritySystem.Services;

public interface IRootSecurityProviderFactory<TDomainObject> : ISecurityProviderFactory<TDomainObject, SecurityRule>;
