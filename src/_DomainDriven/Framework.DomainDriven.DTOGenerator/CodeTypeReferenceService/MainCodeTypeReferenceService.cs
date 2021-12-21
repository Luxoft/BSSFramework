using System;

namespace Framework.DomainDriven.DTOGenerator
{
    public class MainCodeTypeReferenceService<TConfiguration> : DynamicCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public MainCodeTypeReferenceService(TConfiguration configuration)
            : base(configuration, FileType.SimpleDTO, FileType.RichDTO)
        {
        }

        public override Type CollectionType => this.Configuration.ClientEditCollectionType;
    }
}
