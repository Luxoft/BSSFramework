﻿namespace WorkflowSampleSystem.Domain
{
    public enum NamedLockOperation
    {
        [Framework.DomainDriven.BLL.Security.Lock.GlobalLockAttribute(typeof(BusinessUnitAncestorLink))]
        BusinessUnitAncestorLock,
    }
}
