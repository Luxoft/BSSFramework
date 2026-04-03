using Framework.CodeGeneration.Configuration;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.Audit;

public interface IAuditGeneratorConfiguration : ICodeGeneratorConfiguration;

public interface IAuditGeneratorConfiguration<out TEnvironment> : IAuditGeneratorConfiguration, IServiceModelGeneratorConfiguration<TEnvironment>
        where TEnvironment : IAuditGenerationEnvironment;
