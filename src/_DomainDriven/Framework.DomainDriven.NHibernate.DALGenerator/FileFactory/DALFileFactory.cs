using System;
using System.Xml.Linq;

using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;

using JetBrains.Annotations;

namespace Framework.DomainDriven.NHibernate.DALGenerator;

public class DALFileFactory<TConfiguration> : GeneratorConfigurationContainer<TConfiguration>, IRenderingFile<XDocument>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly IMappingGenerator _mappingGenerator;


    public DALFileFactory(TConfiguration configuration, [NotNull] IMappingGenerator mappingGenerator)
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
