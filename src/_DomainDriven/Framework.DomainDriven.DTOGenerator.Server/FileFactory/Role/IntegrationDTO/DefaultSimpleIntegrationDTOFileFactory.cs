using System;

namespace Framework.DomainDriven.DTOGenerator.Server
{
    public class DefaultSimpleIntegrationDTOFileFactory<TConfiguration> : IntegrationDTOFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
    {
        public DefaultSimpleIntegrationDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }


        public override DTOFileType FileType { get; } = ServerFileType.SimpleIntegrationDTO;

        protected override bool HasToDomainObjectMethod => this.IsPersistent();

        protected override bool AllowCreate { get; } = false;
    }
}
