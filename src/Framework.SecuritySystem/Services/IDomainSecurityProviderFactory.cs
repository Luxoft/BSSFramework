using Framework.SecuritySystem.ProviderFactories;

namespace Framework.SecuritySystem.Services;

public interface IDomainSecurityProviderFactory<TDomainObject> : ISecurityProviderFactory<TDomainObject, SecurityRule>;
