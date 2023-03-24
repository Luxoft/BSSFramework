using System.Collections.Generic;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.Generation.Domain;

namespace SampleSystem.CodeGenerate.ServerDTO;

public class SampleSystemServerFileGenerator<TConfiguration> : ServerFileGenerator<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public SampleSystemServerFileGenerator(TConfiguration configuration)
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
