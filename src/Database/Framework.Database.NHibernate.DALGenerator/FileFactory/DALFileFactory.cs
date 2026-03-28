using System.Xml.Linq;

using Framework.Database.NHibernate.DALGenerator._Internal;
using Framework.Database.NHibernate.DALGenerator.Configuration;
using Framework.FileGeneration;

namespace Framework.Database.NHibernate.DALGenerator.FileFactory;

public class DALFileFactory<TConfiguration> : GeneratorConfigurationContainer<TConfiguration>, IRenderingFile<XDocument>
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly IMappingGenerator _mappingGenerator;


    public DALFileFactory(TConfiguration configuration, IMappingGenerator mappingGenerator)
        : base(configuration)
    {
        this._mappingGenerator = mappingGenerator ?? throw new ArgumentNullException(nameof(mappingGenerator));
    }


    public string Filename => $"Generated.{this._mappingGenerator.Assembly.Name}.hbm";


    public XDocument GetRenderData()
    {
        return this._mappingGenerator.Generate();
    }
}
