using System;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Server;

namespace SampleSystem.CodeGenerate.ServerDTO
{
    public abstract class RefDTOFileFactory<TConfiguration> : MainDTOFileFactory<TConfiguration>

        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
    {
        protected RefDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
            this.CodeTypeReferenceService = new FullRefCodeTypeReferenceService<TConfiguration>(this.Configuration);
        }


        public override IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }

        protected override bool HasMapToDomainObjectMethod { get; } = false;
    }
}