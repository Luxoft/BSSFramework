using System;

namespace Framework.Core.Serialization
{
    public class Formatter<TValue, TResult> : IFormatter<TValue, TResult>
    {
        private readonly Func<TValue, TResult> _formatFunc;


        public Formatter(Func<TValue, TResult> formatFunc)
        {
            if (formatFunc == null) throw new ArgumentNullException(nameof(formatFunc));

            this._formatFunc = formatFunc;
        }


        public virtual TResult Format(TValue value)
        {
            return this._formatFunc(value);
        }
    }
}