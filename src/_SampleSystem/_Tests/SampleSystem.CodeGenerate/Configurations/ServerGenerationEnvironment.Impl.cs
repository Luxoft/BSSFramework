using Framework.CodeGeneration.BLLCoreGenerator.Configuration;
using Framework.CodeGeneration.BLLGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.Audit.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.ProjectionGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.Audit;
using Framework.Database.NHibernate.DALGenerator.Configuration;

namespace SampleSystem.CodeGenerate;

public partial class ServerGenerationEnvironment :
        IBLLCoreGenerationEnvironment,
        IbllGenerationEnvironment,
        IDALGenerationEnvironment,
        IProjectionGenerationEnvironment,
        IAuditGenerationEnvironment,
        IAuditDTOGenerationEnvironment
{
    IBLLCoreGeneratorConfiguration<IBLLCoreGenerationEnvironment> IBLLCoreGeneratorConfigurationContainer.BLLCore => this.BLLCore;

    IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment> IServerDTOGeneratorConfigurationContainer.ServerDTO => this.ServerDTO;
}
