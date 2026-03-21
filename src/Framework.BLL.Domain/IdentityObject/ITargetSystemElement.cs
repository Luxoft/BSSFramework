namespace Framework.BLL.Domain.IdentityObject;

public interface ITargetSystemElement<out TTargetSystem>
{
    TTargetSystem TargetSystem { get; }
}
