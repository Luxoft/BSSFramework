namespace Framework.Authorization.TestGenerate;

public partial class ServerGenerationEnvironment :

        DomainDriven.BLLCoreGenerator.IGenerationEnvironmentBase,

        DomainDriven.BLLGenerator.IGenerationEnvironmentBase,

        DomainDriven.DTOGenerator.Server.IServerGenerationEnvironmentBase,

        DomainDriven.ServiceModelGenerator.IGenerationEnvironmentBase,

        DomainDriven.NHibernate.DALGenerator.IGenerationEnvironmentBase
{
    DomainDriven.BLLCoreGenerator.IGeneratorConfigurationBase<DomainDriven.BLLCoreGenerator.IGenerationEnvironmentBase> DomainDriven.BLLCoreGenerator.IGeneratorConfigurationContainer.BLLCore => this.BLLCore;

    DomainDriven.DTOGenerator.Server.IServerGeneratorConfigurationBase<DomainDriven.DTOGenerator.Server.IServerGenerationEnvironmentBase> DomainDriven.DTOGenerator.Server.IGeneratorConfigurationContainer.ServerDTO => this.ServerDTO;
}
