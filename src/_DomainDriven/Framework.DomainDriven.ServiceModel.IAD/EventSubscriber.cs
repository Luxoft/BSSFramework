using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Events;

public class EventSubscriber : IEventSubscriber, IDisposable
{
    private readonly IDBSession session;

    private bool subscribed;

    private bool disposed;

    public EventSubscriber(IDBSession session)
    {
        this.session = session;
    }


    /// <inheritdoc />
    public void Subscribe()
    {
        if (this.subscribed)
        {
            return;
        }

        this.subscribed = true;

        this.InternalSubscribe();
    }


    protected virtual void InternalSubscribe()
    {
        //this.session.Flushed += (_, eventArgs) =>
        //                        {
        //                            var listeners = this.GetDALFlushedListeners().ToArray();

        //                            listeners.Foreach(listener => listener.Process(eventArgs));
        //                        };

        //this.session.BeforeTransactionCompleted += (_, eventArgs) =>
        //                                           {
        //                                               var listeners = this.GetBeforeTransactionCompletedListeners().ToArray();

        //                                               listeners.Foreach(listener => listener.Process(eventArgs));
        //                                           };

        //this.session.AfterTransactionCompleted += (_, eventArgs) =>
        //                                          {
        //                                              var listeners = this.GetAfterTransactionCompletedListeners().ToArray();

        //                                              listeners.Foreach(listener => listener.Process(eventArgs));
        //                                          };

        //this.AuthorizationEventsSubscriptionManager.Maybe(eventManager => eventManager.Subscribe());

        //this.ConfigurationEventsSubscriptionManager.Maybe(eventManager => eventManager.Subscribe());

        //foreach (var module in this.GetModules())
        //{
        //    module.SubscribeEvents();
        //}
    }

    //protected virtual IEnumerable<IDALListener> GetBeforeTransactionCompletedListeners()
    //{
    //    foreach (var module in this.GetModules())
    //    {
    //        foreach (var listener in module.GetBeforeTransactionCompletedListeners())
    //        {
    //            yield return listener;
    //        }
    //    }
    //}

    //protected abstract IEnumerable<IDALListener> GetAfterTransactionCompletedListeners();

    //protected abstract IEnumerable<IDALListener> GetDALFlushedListeners();


    //protected virtual IEventsSubscriptionManager<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase> CreateAuthorizationEventsSubscriptionManager()
    //{
    //    return null;
    //}

    //protected virtual IEventsSubscriptionManager<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase> CreateConfigurationEventsSubscriptionManager()
    //{
    //    return null;
    //}


    ///// <summary>
    ///// Получение авторизацонных DALListener-ов с возможностью ручных вызовов
    ///// </summary>
    ///// <returns></returns>
    //protected virtual IEnumerable<IManualEventDALListener<Framework.Authorization.Domain.PersistentDomainObjectBase>> GetAuthorizationEventDALListeners()
    //{
    //    yield break;
    //}

    ///// <summary>
    ///// Получение конфигурационных DALListener-ов с возможностью ручных вызовов
    ///// </summary>
    ///// <returns></returns>
    //protected virtual IEnumerable<IManualEventDALListener<Framework.Configuration.Domain.PersistentDomainObjectBase>> GetConfigurationEventDALListeners()
    //{
    //    yield break;
    //}

    ///// <inheritdoc />
    //protected override IEnumerable<IManualEventDALListener<Framework.Authorization.Domain.PersistentDomainObjectBase>> GetAuthorizationEventDALListeners()
    //{
    //    foreach (var baseListener in base.GetAuthorizationEventDALListeners())
    //    {
    //        yield return baseListener;
    //    }

    //    yield return this.GetAuthEventDALListener();
    //}

    //private IManualEventDALListener<Framework.Authorization.Domain.PersistentDomainObjectBase> GetAuthEventDALListener()
    //{
    //    var authEventTypes = new[]
    //                         {
    //                                 TypeEvent.Save<Principal>(),
    //                                 TypeEvent.SaveAndRemove<Permission>(),
    //                                 TypeEvent.SaveAndRemove<BusinessRole>()
    //                         };

    //    var dependencyEvents = new[]
    //                           {
    //                                   TypeEventDependency.FromSaveAndRemove<PermissionFilterItem, Permission>(z => z.Permission),
    //                                   TypeEventDependency.FromSaveAndRemove<Permission, Principal>(z => z.Principal)
    //                           };

    //    var messageSender = new AuthorizationLocalDBEventMessageSender(this.Authorization, this.Configuration, "authDALQuery");

    //    return new DefaultAuthDALListener(this.Authorization, authEventTypes, messageSender, dependencyEvents);
    //}

    //protected override IEventsSubscriptionManager<ISampleSystemBLLContext, PersistentDomainObjectBase> CreateMainEventsSubscriptionManager()
    //{
    //    return new SampleSystemEventsSubscriptionManager(
    //                                                     this.MainContext,
    //                                                     new SampleSystemLocalDBEventMessageSender(this.MainContext, this.Configuration)); // Sender для отправки евентов в локальную бд
    //}

    //protected override IEventsSubscriptionManager<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase> CreateAuthorizationEventsSubscriptionManager()
    //{
    //    return new AuthorizationEventsSubscriptionManager(
    //                                                      this.Authorization,
    //                                                      new AuthorizationLocalDBEventMessageSender(this.Authorization, this.Configuration)); // Sender для отправки евентов в локальную бд
    //}

    ///// <summary>
    ///// Добавление подписок на евенты для арибы
    ///// </summary>
    //protected override void SubscribeEvents()
    //{
    //    base.SubscribeEvents();

    //    this.aribaSubscriptionManager.Subscribe();
    //}


    //protected IEventsSubscriptionManager<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase> AuthorizationEventsSubscriptionManager => this.lazyAuthorizationEventsSubscriptionManager.Value;

    //protected IEventsSubscriptionManager<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase> ConfigurationEventsSubscriptionManager => this.lazyConfigurationEventsSubscriptionManager.Value;


    public void Dispose()
    {
    }
}
