using System.Collections.ObjectModel;

using Framework.Async;

namespace Framework.ServiceModel.Async
{
    public interface IAsyncSourceByIdentsService<TIdentityObject, TViewItem>
    {
        IAsyncProcessFunc<ObservableCollection<TIdentityObject>, ObservableCollection<TViewItem>> SourceFunc { get; }
    }
}