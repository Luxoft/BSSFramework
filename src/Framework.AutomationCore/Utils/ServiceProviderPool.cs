using System;
using System.Collections.Concurrent;
using Automation.Utils;
using Automation.Utils.DatabaseUtils;
using Automation.Utils.DatabaseUtils.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Automation;

public abstract class ServiceProviderPool
{
    protected IConfiguration RootConfiguration { get; }
    protected ConfigUtil ConfigUtil { get; }

    private readonly ConcurrentBag<IServiceProvider> providersCache = new ConcurrentBag<IServiceProvider>();

    private readonly Lazy<string> lazyDefaultConnectionString;

    protected ServiceProviderPool(IConfiguration rootRootConfiguration, ConfigUtil configUtil)
    {
        this.RootConfiguration = rootRootConfiguration;
        this.ConfigUtil = configUtil;

        this.lazyDefaultConnectionString = new Lazy<string>(this.BuildDefaultConnectionString);
    }

    protected abstract IServiceProvider Build(IDatabaseContext databaseContext);

    public IServiceProvider Get() => this.providersCache.TryTake(out IServiceProvider provider) ? provider : this.Build(this.BuildDatabaseContext());

    public void Release(IServiceProvider serviceProvider) => this.providersCache.Add(serviceProvider);

    protected virtual DatabaseContext BuildDatabaseContext()
    {
        return new DatabaseContext(
            this.ConfigUtil,
            new DatabaseContextSettings(this.lazyDefaultConnectionString.Value, this.GetSecondaryDatabases()));
    }

    protected virtual string BuildDefaultConnectionString()
    {
        return this.ConfigUtil.GetConnectionString("DefaultConnection");
    }

    protected virtual string[] GetSecondaryDatabases()
    {
        return Array.Empty<string>();
    }
}