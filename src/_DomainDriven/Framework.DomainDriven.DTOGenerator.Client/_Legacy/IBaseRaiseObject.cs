using System.ComponentModel;

namespace Framework.Reactive
{
    internal interface IBaseRaiseObject : INotifyPropertyChanged
    {
        new PropertyChangedEventHandler PropertyChanged { get; }
    }
}
