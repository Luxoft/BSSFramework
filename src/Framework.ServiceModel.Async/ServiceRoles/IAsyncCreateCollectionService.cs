using System.Collections.ObjectModel;

using Framework.Async;

namespace Framework.ServiceModel.Async
{
    public interface IAsyncCreateCollectionService<in TInputData, TChangingObject>
    {
        IAsyncProcessFunc<TInputData, ObservableCollection<TChangingObject>> CreateFunc { get; }
    }
}