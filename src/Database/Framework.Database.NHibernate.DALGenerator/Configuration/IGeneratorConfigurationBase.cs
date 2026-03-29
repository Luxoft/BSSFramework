using Framework.Database.NHibernate.DALGenerator._Internal;

namespace Framework.Database.NHibernate.DALGenerator.Configuration;

public interface IGeneratorConfigurationBase<out TEnvironment> : IGeneratorConfigurationBase, IGeneratorConfiguration<TEnvironment>
        where TEnvironment : IGenerationEnvironmentBase
{
}

public interface IGeneratorConfigurationBase : IGeneratorConfiguration
{
    IEnumerable<IMappingGenerator> GetMappingGenerators();
}
