namespace Framework.BLL.Domain.IdentityObject;

public interface IVersionObject<out TVersion>
{
    TVersion Version { get; }
}
