using Framework.Database.NHibernate.DALGenerator.Internal;
using Framework.FileGeneration.Configuration;

namespace Framework.Database.NHibernate.DALGenerator.Configuration;

public interface IDALGeneratorConfiguration<out TEnvironment> : IDALGeneratorConfiguration, IFileGeneratorConfiguration<TEnvironment>
    where TEnvironment : IDALGenerationEnvironment;

public interface IDALGeneratorConfiguration : IFileGeneratorConfiguration
{
    IEnumerable<IMappingGenerator> GetMappingGenerators();
}
