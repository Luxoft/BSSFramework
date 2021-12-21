using Framework.Async;

namespace Framework.ServiceModel.Async
{
    public interface IAsyncCreateService<in TInputData, out TChangingObject>
    {
        IAsyncProcessFunc<TInputData, TChangingObject> CreateFunc { get; }
    }
}