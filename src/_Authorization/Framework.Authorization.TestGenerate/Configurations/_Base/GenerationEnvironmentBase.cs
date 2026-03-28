using Framework.Authorization.Domain;
using Framework.CodeGeneration.DomainMetadata;

namespace Framework.Authorization.TestGenerate;

public abstract class GenerationEnvironmentBase()
    : GenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, Guid>(
        v => v.Id,
        typeof(DomainObjectChangeModel<>).Assembly);
