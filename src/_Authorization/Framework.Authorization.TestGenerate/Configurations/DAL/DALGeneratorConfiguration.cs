using Framework.DomainDriven.NHibernate.DALGenerator;

namespace Framework.Authorization.TestGenerate
{
    public class DALGeneratorConfiguration : GeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public DALGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {

        }
    }
}