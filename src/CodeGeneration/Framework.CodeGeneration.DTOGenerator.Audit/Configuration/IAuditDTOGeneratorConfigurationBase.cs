using Framework.CodeGeneration.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.Audit.Configuration;

public interface IAuditDTOGeneratorConfigurationBase<out TEnvironment> : IAuditDTOGeneratorConfigurationBase, IGeneratorConfiguration<TEnvironment>
    where TEnvironment : IAuditDTOGenerationEnvironmentBase;

public interface IAuditDTOGeneratorConfigurationBase : IGeneratorConfiguration
{
    string DomainObjectPropertiesRevisionDTOFullTypeName { get; }

    string DomainObjectPropertiesRevisionDTOTypeName { get; }

    string PropertyRevisionTypeName { get; }

    string PropertyRevisionFullTypeName { get; }
}
