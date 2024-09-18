using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate.DALGenerator;

namespace Framework.Configuration.TestGenerate;

public class DALGeneratorConfiguration(ServerGenerationEnvironment environment)
    : GeneratorConfigurationBase<ServerGenerationEnvironment>(environment)
{
    public override DatabaseName DatabaseName => this.Environment.DatabaseName;
}
