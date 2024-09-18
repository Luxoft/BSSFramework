using System.Xml.Linq;

using Framework.Core;
using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.NHibernate.DALGenerator;

public class DALFileGenerator(IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
    : DALFileGenerator<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>(configuration);

public class DALFileGenerator<TConfiguration>(TConfiguration configuration)
    : GeneratorConfigurationContainer<TConfiguration>(configuration), IFileGenerator<IRenderingFile<XDocument>, XDocumentFileRenderer>
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public XDocumentFileRenderer Renderer => XDocumentFileRenderer.Default;


    public IEnumerable<IRenderingFile<XDocument>> GetFileGenerators()
    {
        foreach (var mappingGenerator in this.Configuration.GetMappingGenerators())
        {
            yield return new DALFileFactory<TConfiguration>(this.Configuration, mappingGenerator);
        }
    }
}
