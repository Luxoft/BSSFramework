using System.Collections.ObjectModel;

using Framework.Async;
using Framework.Core;

namespace Framework.ServiceModel.Async;

public interface IAsyncRemoveCollectionService<TIdentityObject>
{
    IAsyncProcessFunc<ObservableCollection<TIdentityObject>, Ignore> RemoveAction { get; }
}
