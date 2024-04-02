using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.NHibernate.DALGenerator;

public interface IGeneratorConfigurationBase<out TEnvironment> : IGeneratorConfigurationBase, IGeneratorConfiguration<TEnvironment>
        where TEnvironment : IGenerationEnvironmentBase
{
    IEscapeWordService EscapeWordService { get; }
}

public interface IGeneratorConfigurationBase : IGeneratorConfiguration
{
    IEnumerable<IMappingGenerator> GetMappingGenerators();
}
