using System.Collections.ObjectModel;

using Framework.Async;
using Framework.Core;

namespace Framework.ServiceModel.Async
{
    public interface IAsyncSourceService<TViewObject>
    {
        IAsyncProcessFunc<Ignore, ObservableCollection<TViewObject>> SourceFunc { get; }
    }
}