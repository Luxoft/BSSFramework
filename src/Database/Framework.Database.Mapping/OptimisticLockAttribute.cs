namespace Framework.Database.Mapping;

[AttributeUsage(AttributeTargets.Class)]
public class OptimisticLockAttribute(OptimisticLockType lockType) : Attribute
{
    public OptimisticLockType LockType { get; private set; } = lockType;
}
