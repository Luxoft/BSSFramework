using System;

namespace Framework.Authorization.TestGenerate;

public partial class ServerGenerationEnvironment :

        Framework.DomainDriven.BLLCoreGenerator.IGenerationEnvironmentBase,

        Framework.DomainDriven.BLLGenerator.IGenerationEnvironmentBase,

        Framework.DomainDriven.DTOGenerator.Server.IServerGenerationEnvironmentBase,

        Framework.DomainDriven.ServiceModelGenerator.IGenerationEnvironmentBase,

        Framework.DomainDriven.NHibernate.DALGenerator.IGenerationEnvironmentBase
{
    Framework.DomainDriven.BLLCoreGenerator.IGeneratorConfigurationBase<Framework.DomainDriven.BLLCoreGenerator.IGenerationEnvironmentBase> Framework.DomainDriven.BLLCoreGenerator.IGeneratorConfigurationContainer.BLLCore => this.BLLCore;

    Framework.DomainDriven.BLLGenerator.IGeneratorConfigurationBase<Framework.DomainDriven.BLLGenerator.IGenerationEnvironmentBase> Framework.DomainDriven.BLLGenerator.IGeneratorConfigurationContainer.BLL => this.BLL;

    Framework.DomainDriven.DTOGenerator.Server.IServerGeneratorConfigurationBase<Framework.DomainDriven.DTOGenerator.Server.IServerGenerationEnvironmentBase> Framework.DomainDriven.DTOGenerator.Server.IGeneratorConfigurationContainer.ServerDTO => this.ServerDTO;
}
