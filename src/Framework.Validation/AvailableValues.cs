using System;

using Framework.Core;

namespace Framework.Validation
{
    public interface IAvailableValues
    {
        Range<T> GetAvailableRange<T>();

        int GetAvailableSize<T>();
    }


    public class AvailableValues : IAvailableValues
    {
        private readonly object _source;


        protected AvailableValues()
        {

        }

        public AvailableValues(object source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this._source = source;
        }


        public virtual Range<T> GetAvailableRange<T>()
        {
            return (this._source as IRangeContainer<T>).Maybe(c => c.Range);
        }

        public virtual int GetAvailableSize<T>()
        {
            return (this._source as ISizeContainer<T>).Maybe(c => c.Size);
        }


        public static readonly AvailableValues Empty = new AvailableValues(new object());

        public static readonly AvailableValues Infinity = new InfinityAvailableValues();


        private class InfinityAvailableValues : AvailableValues
        {
            public override Range<T> GetAvailableRange<T>()
            {
                return Range<T>.Infinity;
            }

            public override int GetAvailableSize<T>()
            {
                return int.MaxValue;
            }
        }
    }
}