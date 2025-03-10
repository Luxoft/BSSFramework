﻿using Microsoft.Extensions.DependencyInjection;

using Framework.Events;
using Framework.SecuritySystem.DependencyInjection;
using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.ApplicationCore;
using Framework.DomainDriven.ApplicationCore.DALListeners;
using Framework.DomainDriven.Lock;

namespace Framework.DomainDriven.Setup;

public class BssFrameworkSettings : IBssFrameworkSettings
{
    private readonly List<Action<IServiceCollection>> registerActions = new();

    private readonly List<IBssFrameworkExtension> extensions = new();

    private Type domainObjectEventMetadataType = typeof(DomainObjectEventMetadata);

    public bool RegisterDenormalizeHierarchicalDALListener { get; set; } = true;

    public IBssFrameworkSettings AddSecuritySystem(Action<ISecuritySystemSettings> setupAction)
    {
        this.registerActions.Add(sc => sc.AddSecuritySystem(setupAction));

        return this;
    }

    public IBssFrameworkSettings AddNamedLocks(Action<IGenericNamedLockSetup> setupAction)
    {
        this.registerActions.Add(sc => sc.RegisterNamedLocks(setupAction));

        return this;
    }

    public IBssFrameworkSettings AddListener<TListener>()
        where TListener : class, IDALListener
    {
        this.registerActions.Add(sc => sc.RegisterListeners(s => s.Add<TListener>()));

        return this;
    }

    public IBssFrameworkSettings AddExtensions(IBssFrameworkExtension extension)
    {
        this.extensions.Add(extension);

        return this;
    }

    public IBssFrameworkSettings SetDomainObjectEventMetadata<T>()
        where T : IDomainObjectEventMetadata
    {
        this.domainObjectEventMetadataType = typeof(T);

        return this;
    }

    public IBssFrameworkSettings AddQueryVisitors<TExpressionVisitorContainerItem>(bool scoped = false)
        where TExpressionVisitorContainerItem : class, IExpressionVisitorContainerItem
    {
        this.registerActions.Add(
            sc =>
            {
                if (scoped)
                {
                    sc.AddScoped<IExpressionVisitorContainerItem, TExpressionVisitorContainerItem>();
                }
                else
                {
                    sc.AddSingleton<IExpressionVisitorContainerItem, TExpressionVisitorContainerItem>();
                }
            });

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        this.InitializeDefault();

        services.AddScoped(typeof(IDomainObjectEventMetadata), this.domainObjectEventMetadataType);

        this.registerActions.ForEach(a => a(services));

        this.extensions.ForEach(ex => ex.AddServices(services));
    }

    private void InitializeDefault()
    {
        if (this.RegisterDenormalizeHierarchicalDALListener)
        {
            this.AddListener<DenormalizeHierarchicalDALListener>();
        }
    }
}
