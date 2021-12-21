using System;
using System.Linq;

using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public partial class StateBLL
    {
        public IQueryable<State> GetForDomainObjectEventAvailable(Type domainObjectType)
        {
            if (domainObjectType == null) throw new ArgumentNullException(nameof(domainObjectType));

            var domainType = this.Context.GetDomainType(domainObjectType);

            return this.GetForDomainObjectEventAvailable(domainType);
        }

        public IQueryable<State> GetForDomainObjectEventAvailable(DomainType domainType)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            return this.GetUnsecureQueryable().Where(s => s.Workflow.Active && s.Workflow.DomainType == domainType && s.DomainObjectEvents.Any());
        }
    }
}