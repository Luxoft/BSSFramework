using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Framework.Core
{
    public static class ObservableCollectionExtensions
    {
        public static ObservableCollection<T> WithRange<T>(this ObservableCollection<T> source, IEnumerable<T> range)
        {
            range.Foreach(source.Add);
            return source;
        }
    }
}