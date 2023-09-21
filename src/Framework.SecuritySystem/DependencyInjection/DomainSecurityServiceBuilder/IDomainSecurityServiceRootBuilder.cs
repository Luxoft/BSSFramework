namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public interface IDomainSecurityServiceRootBuilder
{
    IDomainSecurityServiceRootBuilder Add<TDomainObject>(Action<IDomainSecurityServiceBuilder<TDomainObject>> setup);

    IDomainSecurityServiceRootBuilder AddMetadata<TMetadata>()
        where TMetadata : IDomainSecurityServiceMetadata;
}

public interface IDomainSecurityServiceMetadata
{
    static abstract Type DomainType { get; }
}

public interface IDomainSecurityServiceMetadata<TDomainObject> : IDomainSecurityServiceMetadata
{
    static Type IDomainSecurityServiceMetadata.DomainType { get; } = typeof(TDomainObject);

    static abstract void Setup(IDomainSecurityServiceBuilder<TDomainObject> builder);

    ISecurityProvider<TDomainObject> OverrideSecurityProvider(ISecurityProvider<TDomainObject> baseProvider, BLLSecurityMode securityMode)
    {
        return baseProvider;
    }

    ISecurityProvider<TDomainObject> OverrideSecurityProvider(ISecurityProvider<TDomainObject> baseProvider, SecurityOperation securityOperation)
    {
        return baseProvider;
    }
}
