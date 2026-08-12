using System.Xml.Linq;

using Framework.Core.Rendering;
using Framework.Database.NHibernate.DALGenerator.Configuration;
using Framework.Database.NHibernate.DALGenerator.FileFactory;
using Framework.FileGeneration;

namespace Framework.Database.NHibernate.DALGenerator;

public class DALFileGenerator(IDALGeneratorConfiguration<IDALGenerationEnvironment> configuration)
    : DALFileGenerator<IDALGeneratorConfiguration<IDALGenerationEnvironment>>(configuration);

public class DALFileGenerator<TConfiguration>(TConfiguration configuration)
    : FileGenerator<TConfiguration, IRenderingFile<XDocument>, XDocumentFileRenderer>(configuration), IFileGenerator<IRenderingFile<XDocument>, XDocumentFileRenderer>
    where TConfiguration : class, IDALGeneratorConfiguration<IDALGenerationEnvironment>
{
    public override XDocumentFileRenderer Renderer => XDocumentFileRenderer.Default;

    public override IEnumerable<IRenderingFile<XDocument>> GetFileGenerators()
    {
        foreach (var mappingGenerator in this.Configuration.GetMappingGenerators())
        {
            yield return new DALFileFactory<TConfiguration>(this.Configuration, mappingGenerator);
        }
    }
}
