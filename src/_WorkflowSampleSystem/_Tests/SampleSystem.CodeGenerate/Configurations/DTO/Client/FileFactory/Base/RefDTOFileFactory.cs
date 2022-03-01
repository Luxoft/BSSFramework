using System;
using System.CodeDom;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Client;

namespace SampleSystem.CodeGenerate.ClientDTO
{
    public abstract class RefDTOFileFactory<TConfiguration> : MainDTOFileFactory<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        protected RefDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
            this.CodeTypeReferenceService = new FullRefCodeTypeReferenceService<TConfiguration>(this.Configuration);
        }


        public override IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }
    }
}