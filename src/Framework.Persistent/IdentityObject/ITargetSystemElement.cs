namespace Framework.Persistent
{
    public interface ITargetSystemElement<out TTargetSystem>
    {
        TTargetSystem TargetSystem { get; }
    }
}