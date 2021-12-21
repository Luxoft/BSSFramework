using System;

namespace Framework.DomainDriven.BLL
{
    [Obsolete("Use Framework.DomainDriven.BLL.BLLEventRoleAttribute instead.", true)]
    public class EventOperationAttribute : Attribute
    {
        public EventOperationAttribute(Type eventOperationType)
        {
        }
    }
}
