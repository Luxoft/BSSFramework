using System.Collections.Generic;
using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Client;
using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;

namespace SampleSystem.CodeGenerate.ClientDTO
{
    public class SampleSystemClientFileGenerator<TConfiguration> : ClientFileGenerator<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public SampleSystemClientFileGenerator(TConfiguration configuration)
            : base(configuration)
        {
        }


        protected override IEnumerable<ICodeFileFactory<DTOFileType>> GetDTOFileGenerators()
        {
            foreach (var fileGenerator in base.GetDTOFileGenerators())
            {
                yield return fileGenerator;
            }

            foreach (var dtoType in this.Configuration.DomainTypes)
            {
                yield return new DefaultFullRefDTOFileFactory<TConfiguration>(this.Configuration, dtoType);
                yield return new DefaultSimpleRefFullDetailDTOFileFactory<TConfiguration>(this.Configuration, dtoType);
            }
        }
    }
}
