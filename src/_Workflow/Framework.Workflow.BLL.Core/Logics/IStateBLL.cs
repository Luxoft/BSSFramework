using System;
using System.Linq;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public partial interface IStateBLL
    {
        IQueryable<State> GetForDomainObjectEventAvailable(Type domainObjectType);

        IQueryable<State> GetForDomainObjectEventAvailable(DomainType domainType);
    }
}