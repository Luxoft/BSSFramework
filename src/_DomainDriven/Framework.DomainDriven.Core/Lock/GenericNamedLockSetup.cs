using System.Linq.Expressions;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Lock;

public class GenericNamedLockSetup : IGenericNamedLockSetup
{
    private Action<IServiceCollection>? mainInitAction;

    private readonly List<Action<IServiceCollection>> elementsInitAction = new();

    public IGenericNamedLockSetup SetNameLockType<TGenericNamedLock>(Expression<Func<TGenericNamedLock, string>> namePath)
        where TGenericNamedLock : new()
    {
        this.mainInitAction = services => services.AddScoped<INamedLockService, NamedLockService<TGenericNamedLock>>()
                                                  .AddScoped<INamedLockInitializer, NamedLockInitializer<TGenericNamedLock>>()
                                                  .AddSingleton<INamedLockSource, RootNamedLockSource>()
                                                  .AddSingleton(new GenericNamedLockTypeInfo<TGenericNamedLock>(namePath));

        return this;
    }

    public IGenericNamedLockSetup AddContainer(Type containerType)
    {
        this.elementsInitAction.Add(
            sc => sc.AddKeyedSingleton<INamedLockSource>(RootNamedLockSource.ElementsKey, new NamedLockTypeContainerSource(containerType)));

        return this;
    }

    public IGenericNamedLockSetup AddManual(NamedLock namedLock)
    {
        this.elementsInitAction.Add(
            sc => sc.AddKeyedSingleton<INamedLockSource>(RootNamedLockSource.ElementsKey, new ManualNamedLockSource(namedLock)));

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        if (this.mainInitAction == null)
        {
            throw new InvalidOperationException("Use 'SetNameLockType' method");
        }

        this.mainInitAction(services);

        this.elementsInitAction.ForEach(a => a(services));
    }
}
