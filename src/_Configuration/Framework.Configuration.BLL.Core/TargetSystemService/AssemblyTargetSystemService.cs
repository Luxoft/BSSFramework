using Framework.Configuration.BLL.SubscriptionSystemService3.Services;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Core;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public class AssemblyTargetSystemService<TBLLContext> : TargetSystemService<TBLLContext>
        where TBLLContext : class
{
    /// <summary>
    /// Создаёт экземпляр класса <see cref="AssemblyTargetSystemService{TBLLContext}"/>.
    /// </summary>
    /// <param name="context">Контекст конфигурации.</param>
    /// <param name="targetSystemContext">Контекст целевой системы.</param>
    /// <param name="targetSystem">Целевая система.</param>
    /// <param name="typeResolver">Распознователь типов.</param>
    public AssemblyTargetSystemService(IConfigurationBLLContext context, TBLLContext targetSystemContext, TargetSystem targetSystem, ITypeResolver<string> typeResolver)
            : this(context, targetSystemContext, targetSystem, typeResolver, null)
    {
    }

    /// <summary>
    /// Создаёт экземпляр класса <see cref="AssemblyTargetSystemService{TBLLContext}"/>.
    /// </summary>
    /// <param name="context">Контекст конфигурации.</param>
    /// <param name="targetSystemContext">Контекст целевой системы.</param>
    /// <param name="targetSystem">Целевая система.</param>
    /// <param name="typeResolver">Распознователь типов.</param>
    /// <param name="subscriptionMetadataStore">Хранилище описаний подписок.</param>
    public AssemblyTargetSystemService(
            IConfigurationBLLContext context,
            TBLLContext targetSystemContext,
            TargetSystem targetSystem,
            ITypeResolver<string> typeResolver,
            SubscriptionMetadataStore subscriptionMetadataStore)
            : base(context, targetSystemContext, targetSystem, typeResolver)
    {
        this.SubscriptionService = this.CreateSubscriptionSystemService(
                                                                        context,
                                                                        targetSystemContext,
                                                                        subscriptionMetadataStore);
    }


    public override ISubscriptionSystemService SubscriptionService { get; }


    public override bool IsAssignable(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return this.TypeResolver.GetTypes().Contains(domainType);
    }

    private ISubscriptionSystemService CreateSubscriptionSystemService(
            IConfigurationBLLContext context,
            TBLLContext targetSystemContext,
            SubscriptionMetadataStore subscriptionMetadataStore)
    {
        if (subscriptionMetadataStore == null)
        {
            throw new InvalidOperationException("SubscriptionMetadataStore instance can not be null for use new subscription services.");
        }

        var factory = new SubscriptionServicesFactory<TBLLContext>(context, targetSystemContext, subscriptionMetadataStore);
        return new Framework.Configuration.BLL.SubscriptionSystemService3.SubscriptionSystemService<TBLLContext>(factory);
    }
}
