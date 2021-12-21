using Framework.Async;

namespace Framework.ServiceModel.Async
{
    public interface IAsyncUpdateService<in TSaveObject, out TIdentityObject>
    {
        IAsyncProcessFunc<TSaveObject, TIdentityObject> UpdateFunc { get; }
    }
}