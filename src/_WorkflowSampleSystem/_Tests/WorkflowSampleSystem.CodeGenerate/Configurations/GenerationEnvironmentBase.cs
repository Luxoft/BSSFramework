﻿using System;

using Framework.DomainDriven.Generation.Domain;

using WorkflowSampleSystem.Domain;

namespace WorkflowSampleSystem.CodeGenerate
{
    public abstract class GenerationEnvironmentBase : GenerationEnvironment<DomainObjectBase, PersistentDomainObjectBase, AuditPersistentDomainObjectBase, Guid>
    {

        protected GenerationEnvironmentBase()
            : base(v => v.Id)
        {
        }


        public override Type SecurityOperationCodeType { get; } = typeof(WorkflowSampleSystemSecurityOperationCode);

        public override Type OperationContextType { get; } = typeof(WorkflowSampleSystemOperationContext);
    }
}
