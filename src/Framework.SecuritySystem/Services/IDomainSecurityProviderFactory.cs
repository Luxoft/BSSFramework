namespace Framework.SecuritySystem.Services;

public interface IDomainSecurityProviderFactory<TDomainObject> : ISecurityProviderFactory<TDomainObject, SecurityRule>;
