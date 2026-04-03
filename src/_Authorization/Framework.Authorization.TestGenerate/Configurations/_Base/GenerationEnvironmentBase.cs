using Framework.Authorization.Domain;
using Framework.CodeGeneration.Configuration;

namespace Framework.Authorization.TestGenerate.Configurations._Base;

public abstract class GenerationEnvironmentBase()
    : CodeGenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, Guid>(
        v => v.Id,
        typeof(DomainObjectChangeModel<>).Assembly);
