using System.Collections.Generic;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.NHibernate.DALGenerator;

public interface IGeneratorConfigurationBase<out TEnvironment> : IGeneratorConfigurationBase, IGeneratorConfiguration<TEnvironment>
        where TEnvironment : IGenerationEnvironmentBase
{

}
public interface IGeneratorConfigurationBase : IGeneratorConfiguration
{
    IEnumerable<IMappingGenerator> GetMappingGenerators();
}
