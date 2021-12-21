using System;
using System.Linq;

using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public partial class StartWorkflowDomainObjectConditionBLL
    {
        public IQueryable<StartWorkflowDomainObjectCondition> GetAvailable(Type domainObjectType)
        {
            if (domainObjectType == null) throw new ArgumentNullException(nameof(domainObjectType));

            var domainType = this.Context.GetDomainType(domainObjectType);

            return this.GetAvailable(domainType);
        }

        public IQueryable<StartWorkflowDomainObjectCondition> GetAvailable(DomainType domainType)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            return from startCondition in this.GetUnsecureQueryable()

                   where startCondition.Active && startCondition.Workflow.DomainType == domainType

                   let workflow = startCondition.Workflow

                   where workflow.IsValid && workflow.Active && workflow.Owner == null

                   select startCondition;
        }
    }
}