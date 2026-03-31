using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.FileFactory;

namespace SampleSystem.CodeGenerate.ServerDTO;

public class SampleSystemServerFileGenerator<TConfiguration>(TConfiguration configuration) : ServerFileGenerator<TConfiguration>(configuration)
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
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
