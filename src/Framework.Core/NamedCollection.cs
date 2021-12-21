using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Linq;

namespace Framework.Core
{
    public class NamedCollection<T> : ReadOnlyCollection<T>, INamedCollection<T>
    {
        private readonly Func<T, string> _getName;

        public NamedCollection(IEnumerable<T> list, Func<T, string> getName)
            : base(list.ToList())
        {
            if (getName == null) throw new ArgumentNullException(nameof(getName));

            this._getName = getName;
        }


        protected virtual IEqualityComparer<string> NameComparer
        {
            get { return StringComparer.CurrentCultureIgnoreCase; }
        }


        public T this[string name]
        {
            get
            {
                return this.Where(item => this.NameComparer.Equals(this._getName(item), name))
                           .Single(() => new Exception($"{typeof(T).Name} with name \"{name}\" not found"),
                                   () => new Exception($"To many {typeof(T).Name} elements with name \"{name}\""));
            }
        }
    }
}