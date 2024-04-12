using Framework.Authorization.Domain;
using Framework.DomainDriven.Generation.Domain;
using Framework.SecuritySystem;

namespace Framework.Authorization.TestGenerate;

public abstract class GenerationEnvironmentBase : GenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, Guid>
{
    public readonly string DTODataContractNamespace = "Auth";


    protected GenerationEnvironmentBase()
            : base(v => v.Id, typeof(DomainObjectFilterModel<>).Assembly)
    {
    }

    public override IReadOnlyList<Type> SecurityRuleTypeList { get; } = new[] { typeof(SpecialRoleSecurityRule) };

    public override Type OperationContextType { get; } = typeof(AuthorizationOperationContext);
}
