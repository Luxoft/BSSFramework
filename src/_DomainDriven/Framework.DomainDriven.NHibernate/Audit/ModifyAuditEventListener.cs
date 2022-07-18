using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Framework.DomainDriven.Audit;

using NHibernate.Event;

namespace Framework.DomainDriven.NHibernate.Audit
{
    [System.Obsolete("Use AuditInterceptor instead - #IADFRAME-693")]
    public class ModifyAuditEventListener : AuditEventListenerBase, IPreInsertEventListener, IPreUpdateEventListener
    {
        public ModifyAuditEventListener(IEnumerable<IAuditProperty> auditProperties)
            : base(auditProperties)
        {
        }

        public Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.OnPreInsert(@event));
        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            return this.SetAuditFields(@event, @event.State);
        }

        public Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.OnPreUpdate(@event));
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            return this.SetAuditFields(@event, @event.State);
        }
    }
}
