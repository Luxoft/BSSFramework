using Framework.Async;

namespace Framework.ServiceModel.Async
{
    public interface IAsyncRevisionViewService<in TIdentityObject, in TRevisionIdentity, out TViewObject>
    {
        IAsyncProcessFunc<TIdentityObject, TRevisionIdentity, TViewObject> ViewRevisionFunc { get; }
    }
}