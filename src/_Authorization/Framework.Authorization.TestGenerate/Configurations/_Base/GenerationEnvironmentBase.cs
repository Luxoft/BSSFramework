﻿using Framework.Authorization.Domain;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.Authorization.TestGenerate;

public abstract class GenerationEnvironmentBase : GenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, Guid>
{
    public readonly string DTODataContractNamespace = "Auth";


    protected GenerationEnvironmentBase()
            : base(v => v.Id, typeof(DomainObjectFilterModel<>).Assembly)
    {
    }

    public override List<Type> SecurityRulesTypes { get; } = typeof(AuthorizationSecurityOperation);

    public override Type OperationContextType { get; } = typeof(AuthorizationOperationContext);
}
