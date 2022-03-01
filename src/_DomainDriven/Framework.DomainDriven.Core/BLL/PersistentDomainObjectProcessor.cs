using System;
using Framework.Core;

namespace Framework.DomainDriven.BLL
{
    public abstract class PersistentDomainObjectProcessor<TBLLContext, TPersistentDomainObjectBase, TResult> : BLLContextContainer<TBLLContext>

        where TBLLContext : class
        where TPersistentDomainObjectBase : class
    {
        protected PersistentDomainObjectProcessor(TBLLContext context) : base(context)
        {

        }


        protected abstract ITypeResolver<string> TypeResolver { get; }


        protected abstract TResult Process<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase;


        public TResult Process(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var domainType = this.TypeResolver.Resolve(name, true);

            var genericMethod = new Func<TResult>(this.Process<TPersistentDomainObjectBase>).Method.GetGenericMethodDefinition();

            return (TResult)genericMethod.MakeGenericMethod(new[] { domainType }).Invoke(this, new object[] { });
        }
    }

    public abstract class PersistentDomainObjectProcessor<TBLLContext, TPersistentDomainObjectBase> :
        BLLContextContainer<TBLLContext>

        where TBLLContext : class
        where TPersistentDomainObjectBase : class
    {
        protected PersistentDomainObjectProcessor(TBLLContext context)
            : base(context)
        {

        }


        protected abstract ITypeResolver<string> TypeResolver { get; }


        protected abstract void Process<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase;

        public void Process(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var domainType = this.TypeResolver.Resolve(name, true);

            var genericMethod = new Action(this.Process<TPersistentDomainObjectBase>).Method.GetGenericMethodDefinition();

            genericMethod.MakeGenericMethod(new[] { domainType }).Invoke(this, new object[] { });
        }
    }


    public abstract class TypeResolverDomainObjectProcessor<TBLLContext, TPersistentDomainObjectBase, TResult> : PersistentDomainObjectProcessor<TBLLContext, TPersistentDomainObjectBase, TResult>

        where TBLLContext : class, ITypeResolverContainer<string>
        where TPersistentDomainObjectBase : class
    {
        protected TypeResolverDomainObjectProcessor(TBLLContext context)
            : base(context)
        {

        }


        protected override Framework.Core.ITypeResolver<string> TypeResolver
        {
            get { return this.Context.TypeResolver; }
        }
    }

    public abstract class TypeResolverDomainObjectProcessor<TBLLContext, TPersistentDomainObjectBase> : PersistentDomainObjectProcessor<TBLLContext, TPersistentDomainObjectBase>

        where TBLLContext : class, ITypeResolverContainer<string>
        where TPersistentDomainObjectBase : class
    {
        protected TypeResolverDomainObjectProcessor(TBLLContext context)
            : base(context)
        {

        }


        protected override Framework.Core.ITypeResolver<string> TypeResolver
        {
            get { return this.Context.TypeResolver; }
        }
    }
}
