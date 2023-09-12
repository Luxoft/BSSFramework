using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;
using Framework.Projection;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class ServerFileGenerator : ServerFileGenerator<IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>>
{
    public ServerFileGenerator(IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase> configuration)
            : base(configuration)
    {
    }
}

public class ServerFileGenerator<TConfiguration> : FileGenerator<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public ServerFileGenerator(TConfiguration configuration)
            : base(configuration)
    {
    }


    protected override ICodeFileFactory<DTOFileType> GetIdentityDTOFileFactory(Type domainType)
    {
        return new DefaultServerIdentityDTOFileFactory<TConfiguration>(this.Configuration, domainType);
    }

    protected override ICodeFileFactory<RoleFileType> GetDomainObjectSecurityOperationCodeFileFactory(Type domainType, IEnumerable<SecurityOperation> securityOperations)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (securityOperations == null) throw new ArgumentNullException(nameof(securityOperations));

        return new DefaultServerDomainObjectSecurityOperationCodeFileFactory<TConfiguration>(this.Configuration, domainType, securityOperations);
    }

    protected virtual ICodeFileFactory<DTOFileType> GetVisualDTOFileFactory(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return new DefaultVisualDTOFileFactory<TConfiguration>(this.Configuration, domainType);
    }

    protected virtual ICodeFileFactory<DTOFileType> GetSimpleDTOFileFactory(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return new DefaultSimpleDTOFileFactory<TConfiguration>(this.Configuration, domainType);
    }

    protected virtual ICodeFileFactory<DTOFileType> GetFullDTOFileFactory(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return new DefaultFullDTOFileFactory<TConfiguration>(this.Configuration, domainType);
    }

    protected virtual ICodeFileFactory<DTOFileType> GetRichDTOFileFactory(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return new DefaultRichDTOFileFactory<TConfiguration>(this.Configuration, domainType);
    }



    protected sealed override IEnumerable<ICodeFile> GetInternalFileGenerators()
    {
        var baseGenerators = base.GetInternalFileGenerators().ToList();

        foreach (var generator in baseGenerators)
        {
            yield return generator;
        }
        //--------------------------------------------------------------------------------
        yield return new LambdaHelperFileFactory<TConfiguration>(this.Configuration);
        //--------------------------------------------------------------------------------

        var externalMappingGenerators = baseGenerators.OfType<IServerMappingServiceExternalMethodGenerator>().ToList();

        yield return new ServerDTOMappingServiceInterfaceFileFactory<TConfiguration>(this.Configuration, externalMappingGenerators);
        yield return new ServerPrimitiveDTOMappingServiceBaseFileFactory<TConfiguration>(this.Configuration, externalMappingGenerators);
        yield return new ServerPrimitiveDTOMappingServiceFileFactory<TConfiguration>(this.Configuration);
        //--------------------------------------------------------------------------------
    }


    protected override IEnumerable<ICodeFileFactory<DTOFileType>> GetDTOFileGenerators()
    {
        foreach (var baseFileFactory in base.GetDTOFileGenerators())
        {
            yield return baseFileFactory;
        }

        yield return new DefaultBaseAbstractDTOFileFactory<TConfiguration>(this.Configuration);
        yield return new DefaultBasePersistentDTOFileFactory<TConfiguration>(this.Configuration);
        yield return new DefaultBaseAuditPersistentDTOFileFactory<TConfiguration>(this.Configuration);

        yield return new DefaultBaseEventDTOFileFactory<TConfiguration>(this.Configuration);

        foreach (var domainType in this.Configuration.DomainTypes)
        {
            if (domainType.IsProjection())
            {
                yield return new DefaultProjectionDTOFileFactory<TConfiguration>(this.Configuration, domainType);
            }
            else
            {
                if (domainType.HasVisualIdentityProperties())
                {
                    yield return this.GetVisualDTOFileFactory(domainType);
                }

                yield return this.GetSimpleDTOFileFactory(domainType);
                yield return this.GetFullDTOFileFactory(domainType);
                yield return this.GetRichDTOFileFactory(domainType);

                yield return new DefaultStrictDTOFileFactory<TConfiguration>(this.Configuration, domainType);

                if (this.Configuration.IsPersistentObject(domainType))
                {
                    yield return new DefaultUpdateDTOFileFactory<TConfiguration>(this.Configuration, domainType);
                }

                yield return new DefaultRichIntegrationDTOFileFactory<TConfiguration>(this.Configuration, domainType);
                yield return new DefaultSimpleIntegrationDTOFileFactory<TConfiguration>(this.Configuration, domainType);

                foreach (var eventOperationCode in domainType.GetEventOperations(typeof(BLLBaseOperation)))
                {
                    yield return new DefaultDomainOperationEventDTOFileFactory<TConfiguration>(this.Configuration, domainType, eventOperationCode);
                }

                yield return new DefaultRichEventDTOFileFactory<TConfiguration>(this.Configuration, domainType);
                yield return new DefaultSimpleEventDTOFileFactory<TConfiguration>(this.Configuration, domainType);
            }
        }
    }
}
