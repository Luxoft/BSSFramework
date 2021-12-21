using System;

namespace Framework.Core
{
    public interface IDynamicSource
    {
        T GetValue<T>()
            where T : class;
    }

    public static class DynamicSourceExtensions
    {
        public static T GetValue<T>(this IDynamicSource dynamicSource, bool raiseFailIfNotFound)
            where T : class
        {
            if (dynamicSource == null) throw new ArgumentNullException(nameof(dynamicSource));

            var result = dynamicSource.GetValue<T>();

            if (result == null && raiseFailIfNotFound)
            {
                throw new Exception($"Object of type {typeof(T).Name} not found");
            }
            else
            {
                return result;
            }
        }

        public static IDynamicSource Add (this IDynamicSource source, IDynamicSource otherSource)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (otherSource == null) throw new ArgumentNullException(nameof(otherSource));


            return new MixedDynamicSource(source, otherSource);
        }

        private class MixedDynamicSource : IDynamicSource
        {
            private readonly IDynamicSource _source;
            private readonly IDynamicSource _otherSource;

            public MixedDynamicSource (IDynamicSource source, IDynamicSource otherSource)
            {
                if (source == null) throw new ArgumentNullException(nameof(source));
                if (otherSource == null) throw new ArgumentNullException(nameof(otherSource));

                this._source = source;
                this._otherSource = otherSource;
            }


            public T GetValue<T>()
                where T : class
            {
                return this._source.GetValue<T>() ?? this._otherSource.GetValue<T>();
            }
        }
    }
}