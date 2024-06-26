﻿using Framework.Authorization.Domain;
using Framework.DomainDriven.Generation.Domain;
using Framework.SecuritySystem;

namespace Framework.Authorization.TestGenerate;

public abstract class GenerationEnvironmentBase()
    : GenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, Guid>(
        v => v.Id,
        typeof(DomainObjectChangeModel<>).Assembly)
{
    public readonly string DTODataContractNamespace = "Auth";

    public override IReadOnlyList<Type> SecurityRuleTypeList { get; } = new[] { typeof(SecurityRole) };

    public override Type OperationContextType { get; } = typeof(AuthorizationOperationContext);
}
