using System;
using System.Linq;

using Framework.Workflow.Domain.Definition;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public partial interface IStartWorkflowDomainObjectConditionBLL
    {
        IQueryable<StartWorkflowDomainObjectCondition> GetAvailable([NotNull]Type domainObjectType);

        IQueryable<StartWorkflowDomainObjectCondition> GetAvailable([NotNull]DomainType domainType);
    }
}
