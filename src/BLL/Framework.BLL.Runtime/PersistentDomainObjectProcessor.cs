using CommonFramework;

using Framework.Core.TypeResolving;

namespace Framework.BLL;

public abstract class PersistentDomainObjectProcessor<TBLLContext, TPersistentDomainObjectBase, TResult>(TBLLContext context) : BLLContextContainer<TBLLContext>(context)
    where TBLLContext : class
    where TPersistentDomainObjectBase : class
{
    protected abstract ITypeResolver<string> TypeResolver { get; }


    protected abstract TResult Process<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase;


    public TResult Process(string name)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));

        var domainType = this.TypeResolver.Resolve(name);

        var genericMethod = new Func<TResult>(this.Process<TPersistentDomainObjectBase>).Method.GetGenericMethodDefinition();

        return genericMethod.MakeGenericMethod(domainType).Invoke<TResult>(this, []);
    }
}

public abstract class PersistentDomainObjectProcessor<TBLLContext, TPersistentDomainObjectBase>(TBLLContext context) :
    BLLContextContainer<TBLLContext>(context)
    where TBLLContext : class
    where TPersistentDomainObjectBase : class
{
    protected abstract ITypeResolver<string> TypeResolver { get; }


    protected abstract void Process<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase;

    public void Process(string name)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));

        var domainType = this.TypeResolver.Resolve(name);

        var genericMethod = new Action(this.Process<TPersistentDomainObjectBase>).Method.GetGenericMethodDefinition();

        genericMethod.MakeGenericMethod(domainType).Invoke(this, []);
    }
}


public abstract class TypeResolverDomainObjectProcessor<TBLLContext, TPersistentDomainObjectBase, TResult>(TBLLContext context)
    : PersistentDomainObjectProcessor<TBLLContext, TPersistentDomainObjectBase, TResult>(context)
    where TBLLContext : class, ITypeResolverContainer<string>
    where TPersistentDomainObjectBase : class
{
    protected override ITypeResolver<string> TypeResolver
    {
        get { return this.Context.TypeResolver; }
    }
}

public abstract class TypeResolverDomainObjectProcessor<TBLLContext, TPersistentDomainObjectBase>(TBLLContext context)
    : PersistentDomainObjectProcessor<TBLLContext, TPersistentDomainObjectBase>(context)
    where TBLLContext : class, ITypeResolverContainer<string>
    where TPersistentDomainObjectBase : class
{
    protected override ITypeResolver<string> TypeResolver
    {
        get { return this.Context.TypeResolver; }
    }
}
