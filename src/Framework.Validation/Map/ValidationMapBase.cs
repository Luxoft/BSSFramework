using System;

using Framework.Core;

namespace Framework.Validation
{
    public abstract class ValidationMapBase : IValidationMap
    {
        private readonly IDictionaryCache<Type, IClassValidationMap> _cache;


        protected ValidationMapBase(IDynamicSource extendedValidationData)
        {
            this.ExtendedValidationData = extendedValidationData ?? throw new ArgumentNullException(nameof(extendedValidationData));

            this.AvailableValues = LazyInterfaceImplementHelper.CreateProxy(() => this.ExtendedValidationData.GetValue<IAvailableValues>(true));


            this._cache = new LazyImplementDictionaryCache<Type, IClassValidationMap>(type =>
            {
                var func = new Func<IClassValidationMap<Ignore>>(this.GetInternalClassMap<Ignore>);

                var method = func.CreateGenericMethod(type);

                return method.Invoke<IClassValidationMap>(this);
            }, type => typeof(IClassValidationMap<>).MakeGenericType(type)).WithLock();
        }


        public IDynamicSource ExtendedValidationData { get; }

        protected IAvailableValues AvailableValues { get; }


        public IClassValidationMap GetClassMap(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return this._cache[type];
        }

        public IClassValidationMap<TSource> GetClassMap<TSource>()
        {
            return (IClassValidationMap<TSource>)this.GetClassMap(typeof(TSource));
        }

        protected IClassValidationMap<TSource> GetClassMap<TSource>(bool lazy)
        {
            return lazy ? LazyInterfaceImplementHelper.CreateProxy(() => this.GetClassMap<TSource>())
                        : this.GetClassMap<TSource>();
        }

        protected abstract IClassValidationMap<TSource> GetInternalClassMap<TSource>();
    }
}
