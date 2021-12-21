using Framework.Async;

namespace Framework.ServiceModel.Async
{
    public interface IAsyncSaveService<in TSaveObject, out TIdentityObject>
    {
        IAsyncProcessFunc<TSaveObject, TIdentityObject> SaveFunc { get; }
    }
}