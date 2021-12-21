using System.Collections.Generic;
using System.Xml.Linq;

using Framework.Core;
using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.NHibernate.DALGenerator
{
    public class DALFileGenerator : DALFileGenerator<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>
    {
        public DALFileGenerator(IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
            : base(configuration)
        {
        }
    }

    public class DALFileGenerator<TConfiguration> : GeneratorConfigurationContainer<TConfiguration>, IFileGenerator<IRenderingFile<XDocument>, XDocumentFileRenderer>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public DALFileGenerator(TConfiguration configuration)
            : base(configuration)
        {
        }


        public XDocumentFileRenderer Renderer => XDocumentFileRenderer.Default;


        public IEnumerable<IRenderingFile<XDocument>> GetFileGenerators()
        {
            foreach (var mappingGenerator in this.Configuration.GetMappingGenerators())
            {
                yield return new DALFileFactory<TConfiguration>(this.Configuration, mappingGenerator);
            }
        }
    }
}