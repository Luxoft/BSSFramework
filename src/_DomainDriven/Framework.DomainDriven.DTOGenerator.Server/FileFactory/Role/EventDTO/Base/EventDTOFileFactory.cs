using System;

namespace Framework.DomainDriven.DTOGenerator.Server;

public abstract class EventDTOFileFactory<TConfiguration> : RoleDTOFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    protected EventDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
        this.CodeTypeReferenceService = new CryptCodeTypeReferenceService<TConfiguration>(this.Configuration, ServerFileType.SimpleEventDTO, ServerFileType.RichEventDTO);
    }


    public override IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }


    protected override string DataContractNamespace => this.Configuration.EventDataContractNamespace;
}
