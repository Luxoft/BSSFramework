using Framework.CodeGeneration.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.Audit.Configuration;

public interface IAuditDTOGeneratorConfiguration<out TEnvironment> : IAuditDTOGeneratorConfiguration, ICodeGeneratorConfiguration<TEnvironment>
    where TEnvironment : IAuditDTOGenerationEnvironment;

public interface IAuditDTOGeneratorConfiguration : ICodeGeneratorConfiguration
{
    string DomainObjectPropertiesRevisionDTOFullTypeName { get; }

    string DomainObjectPropertiesRevisionDTOTypeName { get; }

    string PropertyRevisionTypeName { get; }

    string PropertyRevisionFullTypeName { get; }
}
