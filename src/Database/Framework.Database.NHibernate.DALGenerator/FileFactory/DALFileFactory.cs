using System.Xml.Linq;

using Framework.Database.NHibernate.DALGenerator._Internal;
using Framework.Database.NHibernate.DALGenerator.Configuration;
using Framework.FileGeneration;
using Framework.FileGeneration.Configuration;

namespace Framework.Database.NHibernate.DALGenerator.FileFactory;

public class DALFileFactory<TConfiguration>(TConfiguration configuration, IMappingGenerator mappingGenerator)
    : GeneratorConfigurationContainer<TConfiguration>(configuration), IRenderingFile<XDocument>
    where TConfiguration : class, IDALGeneratorConfiguration<IDALGenerationEnvironment>
{
    public string Filename => $"Generated.{mappingGenerator.Assembly.GetName().Name}.hbm";


    public XDocument GetRenderData() => mappingGenerator.Generate();
}
