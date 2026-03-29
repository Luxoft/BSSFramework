using Framework.CodeGeneration.DomainMetadata;
using Framework.Configuration.Domain;

using SecuritySystem;

namespace Framework.Configuration.TestGenerate.Configurations._Base;

public abstract class GenerationEnvironmentBase()
    : GenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, Guid>(v => v.Id, typeof(DomainObjectFilterModel<>).Assembly)
{
    public readonly string DTODataContractNamespace = "Configuration";

    public override IReadOnlyList<Type> SecurityRuleTypeList { get; } = [typeof(SecurityRole)];
}
