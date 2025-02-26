using Framework.SecuritySystem.ProviderFactories;

namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public interface IDomainSecurityServiceMetadata<TDomainObject> :
    IDomainSecurityServiceMetadata,
    ISecurityProviderInjector<TDomainObject, SecurityRule.ModeSecurityRule>,
    ISecurityProviderInjector<TDomainObject, DomainSecurityRule.OperationSecurityRule>,
    ISecurityProviderInjector<TDomainObject, DomainSecurityRule.NonExpandedRolesSecurityRule>,
    ISecurityProviderInjector<TDomainObject, DomainSecurityRule.ExpandedRolesSecurityRule>
{
    static Type IDomainSecurityServiceMetadata.DomainType { get; } = typeof(TDomainObject);

    static abstract void Setup(IDomainSecurityServiceBuilder<TDomainObject> builder);
}

public interface IDomainSecurityServiceMetadata
{
    static abstract Type DomainType { get; }
}
