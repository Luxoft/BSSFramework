using System;
using System.Linq;

using Framework.DomainDriven.BLL;
using Framework.Workflow.Domain.Definition;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public partial interface IWorkflowBLL : IPathBLL<Framework.Workflow.Domain.Definition.Workflow>
    {
        IQueryable<Framework.Workflow.Domain.Definition.Workflow> GetForActiveLambdaAvailable([NotNull]Type domainObjectType);

        IQueryable<Framework.Workflow.Domain.Definition.Workflow> GetForActiveLambdaAvailable([NotNull]DomainType domainType);


        IQueryable<Framework.Workflow.Domain.Definition.Workflow> GetForAutoRemovingAvailable([NotNull]Type domainObjectType);

        IQueryable<Framework.Workflow.Domain.Definition.Workflow> GetForAutoRemovingAvailable([NotNull]DomainType domainType);
    }
}
