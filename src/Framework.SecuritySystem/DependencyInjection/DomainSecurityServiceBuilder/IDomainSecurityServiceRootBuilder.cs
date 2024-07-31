﻿namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public interface IDomainSecurityServiceRootBuilder
{
    bool AutoAddSelfRelativePath { get; set; }

    IDomainSecurityServiceRootBuilder Add<TDomainObject>(Action<IDomainSecurityServiceBuilder<TDomainObject>> setup);

    IDomainSecurityServiceRootBuilder AddMetadata<TMetadata>()
        where TMetadata : IDomainSecurityServiceMetadata;
}
