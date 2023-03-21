using Framework.Async;

namespace Framework.ServiceModel.Async;

public interface IAsyncViewService<in TIdentityObject, out TViewObject>
{
    IAsyncProcessFunc<TIdentityObject, TViewObject> ViewFunc { get; }
}
