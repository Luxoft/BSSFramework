namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public interface IDomainSecurityServiceRootBuilder
{
    /// <summary>
    /// Автоматическое добавление относительных путей на самих себя (v => v)
    /// </summary>
    bool AutoAddSelfRelativePath { get; set; }

    IDomainSecurityServiceRootBuilder Add<TDomainObject>(Action<IDomainSecurityServiceBuilder<TDomainObject>> setup);

    IDomainSecurityServiceRootBuilder AddMetadata<TMetadata>()
        where TMetadata : IDomainSecurityServiceMetadata;
}
