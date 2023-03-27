using Framework.Configuration.Domain;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.Configuration.TestGenerate;

public abstract class GenerationEnvironmentBase : GenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, Guid>
{
    public readonly string DTODataContractNamespace = "Configuration";


    protected GenerationEnvironmentBase()
            : base(v => v.Id, typeof(DomainObjectFilterModel<>).Assembly)
    {
    }

    public override Type SecurityOperationCodeType { get; } = typeof(ConfigurationSecurityOperationCode);

    public override Type OperationContextType { get; } = typeof(ConfigurationOperationContext);
}
