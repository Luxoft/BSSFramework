using Framework.Async;
using Framework.Core;

namespace Framework.ServiceModel.Async;

public interface IAsyncRemoveService<in TIdentityObject>
{
    IAsyncProcessFunc<TIdentityObject, Ignore> RemoveAction { get; }
}
