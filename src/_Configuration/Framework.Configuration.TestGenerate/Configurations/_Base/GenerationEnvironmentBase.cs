using Framework.CodeGeneration.Configuration;
using Framework.Configuration.Domain;

namespace Framework.Configuration.TestGenerate.Configurations._Base;

public abstract class GenerationEnvironmentBase()
    : CodeGenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, Guid>(v => v.Id, typeof(DomainObjectFilterModel<>).Assembly)
{
    public readonly string DTODataContractNamespace = "Configuration";
}
