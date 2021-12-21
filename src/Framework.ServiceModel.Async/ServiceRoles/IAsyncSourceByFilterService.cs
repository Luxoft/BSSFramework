using System.Collections.ObjectModel;

using Framework.Async;

namespace Framework.ServiceModel.Async
{
    public interface IAsyncSourceByFilterService<in TFilter, TViewItem>
    {
        IAsyncProcessFunc<TFilter, ObservableCollection<TViewItem>> SourceFunc { get; }
    }
}