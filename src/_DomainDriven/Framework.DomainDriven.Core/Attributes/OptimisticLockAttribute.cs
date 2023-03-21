using System;

namespace Framework.DomainDriven.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class OptimisticLockAttribute : Attribute
{
    public OptimisticLockType LockType { get; private set; }
    public OptimisticLockAttribute(OptimisticLockType lockType)
    {
        this.LockType = lockType;
    }
}
