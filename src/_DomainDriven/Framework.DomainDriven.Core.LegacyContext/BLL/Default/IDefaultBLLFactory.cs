using Framework.Persistent;

namespace Framework.DomainDriven.BLL;

public interface IDefaultBLLFactory<in TPersistentDomainObjectBase, TIdent>
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    IDefaultDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent> Create<TDomainObject>()
        where TDomainObject : class, TPersistentDomainObjectBase;
}

public interface IBLLFactoryContainer<out TFactory>
{
    TFactory Default { get; }

    TFactory Implemented { get; }
}

public interface IBLLFactoryInitializer
{
    static abstract void RegisterBLLFactory(Microsoft.Extensions.DependencyInjection.IServiceCollection serviceCollection);
}
