using System;
using System.Collections.Generic;

using Framework.Authorization;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Workflow.BLL;

using JetBrains.Annotations;

using WorkflowSampleSystem.BLL;
using WorkflowSampleSystem.Domain;

namespace WorkflowSampleSystem.ServiceEnvironment;

public class WorkflowSamplSystemBLLContextContainerModule : WorkflowBLLContextContainerModule<WorkflowSampleSystemServiceEnvironment, WorkflowSampleSystemBLLContextContainer, IWorkflowSampleSystemBLLContext, PersistentDomainObjectBase, WorkflowSampleSystemSecurityOperationCode>
{
    public WorkflowSamplSystemBLLContextContainerModule([NotNull] WorkflowSampleSystemServiceEnvironment mainServiceEnvironment, WorkflowSampleSystemBLLContextContainer bllContextContainer)
            : base(mainServiceEnvironment.WorkflowModule, mainServiceEnvironment, bllContextContainer)
    {
    }

    protected override IEnumerable<ITargetSystemService> GetWorkflowTargetSystemServices()
    {
        yield return this.GetMainWorkflowTargetSystemService(new HashSet<Type>(new[] { typeof(Domain.Location) }));

        yield return new Framework.Workflow.BLL.TargetSystemService<IWorkflowSampleSystemAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase, AuthorizationSecurityOperationCode>(
         this.Workflow,
         this.BllContextContainer.WorkflowSampleSystemAuthorization,
         this.Workflow.Logics.TargetSystem.GetByName(TargetSystemHelper.AuthorizationName, true),
         this.MainServiceEnvironment.WorkflowAuthorizationSystemCompileCache,
         new[] { typeof(Framework.Authorization.Domain.Permission) });
    }
}
