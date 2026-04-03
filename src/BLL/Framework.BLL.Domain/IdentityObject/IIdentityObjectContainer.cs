namespace Framework.BLL.Domain.IdentityObject;

public interface IIdentityObjectContainer<out TIdentityObject>
{
    TIdentityObject Identity { get; }
}
