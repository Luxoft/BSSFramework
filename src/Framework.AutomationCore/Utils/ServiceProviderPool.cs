using System;
using System.Linq;
using System.Collections.Generic;
using Automation.Utils;
using Automation.Utils.DatabaseUtils;
using Automation.Utils.DatabaseUtils.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Automation;

public abstract class ServiceProviderPool
{
    protected IConfiguration RootConfiguration { get; }
    protected ConfigUtil ConfigUtil { get; }

    private readonly object locker = new object();

    private readonly Dictionary<IServiceProvider, bool> providersCache = new Dictionary<IServiceProvider, bool>();

    private readonly Lazy<string> lazyDefaultConnectionString;

    protected ServiceProviderPool(IConfiguration rootRootConfiguration, ConfigUtil configUtil)
    {
        this.RootConfiguration = rootRootConfiguration;
        this.ConfigUtil = configUtil;

        this.lazyDefaultConnectionString = new Lazy<string>(this.BuildDefaultConnectionString);
    }

    protected abstract IServiceProvider Build(IDatabaseContext databaseContext);

    public IServiceProvider Get()
    {
        lock (this.locker)
        {
            var provider = this.GetOrCreateInternal();
            this.providersCache[provider] = true;
            return provider;
        }
    }

    public void Release(IServiceProvider serviceProvider)
    {
        lock (this.locker)
        {
            this.providersCache[serviceProvider] = false;
        }
    }

    private IServiceProvider GetOrCreateInternal()
    {
        var freeRequest =

            from pair in this.providersCache
            where !pair.Value
            select pair.Key;

        var firstFree = freeRequest.FirstOrDefault();

        if (firstFree == null)
        {
            return this.Build(this.BuildDatabaseContext());
        }

        return firstFree;
    }

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
