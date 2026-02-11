using Framework.Configuration.Domain;
using Framework.DomainDriven.Generation.Domain;
using SecuritySystem;

namespace Framework.Configuration.TestGenerate;

public abstract class GenerationEnvironmentBase()
    : GenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, Guid>(v => v.Id, typeof(DomainObjectFilterModel<>).Assembly)
{
    public readonly string DTODataContractNamespace = "Configuration";

    public override IReadOnlyList<Type> SecurityRuleTypeList { get; } = [typeof(SecurityRole)];
}
