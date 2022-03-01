using System.ComponentModel;

namespace Framework.Reactive
{
    public interface IBaseRaiseObject : INotifyPropertyChanged
    {
        new PropertyChangedEventHandler PropertyChanged { get; }
    }
}
