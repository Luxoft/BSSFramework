using System;
using System.CodeDom;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DTOGenerator;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator
{
    public abstract class IntegrationBaseSaveMethodGenerator<TConfiguration> : IntegrationMethodGenerator<TConfiguration, BLLIntegrationSaveRoleAttribute>
        where TConfiguration : class, IIntegrationGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        protected readonly CodeTypeReference IdentTypeRef;

        protected readonly CodeTypeReference RichIntegrationTypeRef;


        protected IntegrationBaseSaveMethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
            this.IdentTypeRef = this.Configuration
                                    .Environment.ServerDTO
                                    .GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO);

            this.RichIntegrationTypeRef = this.Configuration
                                              .Environment.ServerDTO
                                              .GetCodeTypeReference(this.DomainType, DTOGenerator.Server.ServerFileType.RichIntegrationDTO);
        }
    }
}
