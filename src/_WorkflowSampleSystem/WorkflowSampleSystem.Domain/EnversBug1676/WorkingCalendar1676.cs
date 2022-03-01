using System;

using Framework.DomainDriven.BLL;
using Framework.Persistent;

namespace WorkflowSampleSystem.Domain.EnversBug1676
{
    [BLLViewRole]
    [WorkflowSampleSystemViewDomainObject(WorkflowSampleSystemSecurityOperationCode.Disabled)]
    public class WorkingCalendar1676 : BaseDirectory
    {
        private readonly Location1676 location;

        public WorkingCalendar1676(Location1676 location)
        {
            this.location = location ?? throw new ArgumentNullException(nameof(location));
        }

        protected WorkingCalendar1676()
        {
        }

        public virtual Location1676 Location => this.location;
    }
}
