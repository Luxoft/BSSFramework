using System.Linq.Expressions;

using CommonFramework.DependencyInjection;

using Framework.Application.Lock;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Application.DependencyInjection;

public class GenericNamedLockBuilder : IGenericNamedLockBuilder, IServiceInitializer
{
    private Action<IServiceCollection>? mainInitAction;

    private readonly List<Action<IServiceCollection>> elementsInitAction = [];

    public IGenericNamedLockBuilder SetNameLockType<TGenericNamedLock>(Expression<Func<TGenericNamedLock, string>> namePath)
        where TGenericNamedLock : new()
    {
        this.mainInitAction = services => services.AddScoped<INamedLockService, NamedLockService<TGenericNamedLock>>()
                                                  .AddScoped<INamedLockInitializer, NamedLockInitializer<TGenericNamedLock>>()
                                                  .AddSingleton<INamedLockSource, RootNamedLockSource>()
                                                  .AddSingleton(new GenericNamedLockTypeInfo<TGenericNamedLock>(namePath));

        return this;
    }

    public IGenericNamedLockBuilder AddContainer(Type containerType)
    {
        this.elementsInitAction.Add(
            sc => sc.AddKeyedSingleton<INamedLockSource>(RootNamedLockSource.ElementsKey, new NamedLockTypeContainerSource(containerType)));

        return this;
    }

    public IGenericNamedLockBuilder AddManual(NamedLock namedLock)
    {
        this.elementsInitAction.Add(
            sc => sc.AddKeyedSingleton<INamedLockSource>(RootNamedLockSource.ElementsKey, new ManualNamedLockSource(namedLock)));

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        if (this.mainInitAction == null)
        {
            throw new InvalidOperationException($"Use '{nameof(this.SetNameLockType)}' method");
        }

        this.mainInitAction(services);

        this.elementsInitAction.ForEach(a => a(services));
    }
}
