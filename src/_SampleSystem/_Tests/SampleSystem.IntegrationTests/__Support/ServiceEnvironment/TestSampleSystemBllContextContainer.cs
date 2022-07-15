﻿using System;
using System.Collections.Generic;

using Framework.Configuration.BLL;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.Notification.DTO;
using Framework.Security.Cryptography;

using SampleSystem.Domain;
using SampleSystem.ServiceEnvironment;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public class TestSampleSystemBllContextContainer : SampleSystemBLLContextContainer
{
    public TestSampleSystemBllContextContainer(
            SampleSystemServiceEnvironment serviceEnvironment,
            IServiceProvider scopedServiceProvider,
            IDBSession dbSession)

            : base(serviceEnvironment, scopedServiceProvider, dbSession)
    {
    }

    protected override IMessageSender<NotificationEventDTO> GetMessageTemplateSender()
    {
        return new LocalDBNotificationEventDTOMessageSender(this.Configuration);
    }

    protected override IEnumerable<IDALListener> GetBeforeTransactionCompletedListeners()
    {
        foreach (var dalListener in base.GetBeforeTransactionCompletedListeners())
        {
            yield return dalListener;
        }

        yield return new PermissionWorkflowDALListener(this.MainContext);
    }
}
