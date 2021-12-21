using System;

namespace NHibernate.Linq.Visitors
{
    internal class Immutable<T>
        where T : class
    {
        private T source;

        public T Value
        {
            get => this.source;

            set
            {
                if (this.source != null && value != this.source)
                {
                    throw new ArgumentException("Value also initialized");
                }

                this.source = value;
            }
        }

        public bool IsInit => this.source != null;
    }
}
