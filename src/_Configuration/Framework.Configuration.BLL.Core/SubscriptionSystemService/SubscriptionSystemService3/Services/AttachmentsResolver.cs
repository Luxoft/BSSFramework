using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;

using JetBrains.Annotations;

using Attachment = System.Net.Mail.Attachment;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Services;

public class AttachmentsResolver<TBLLContext>
        where TBLLContext : class
{
    private readonly LambdaProcessorFactory<TBLLContext> lambdaProcessorFactory;

    public AttachmentsResolver([NotNull] LambdaProcessorFactory<TBLLContext> lambdaProcessorFactory)
    {
        this.lambdaProcessorFactory = lambdaProcessorFactory;
    }

    public virtual IEnumerable<Attachment> Resolve<T>(
            [NotNull] Subscription subscription,
            [NotNull] DomainObjectVersions<T> versions)
            where T : class
    {
        if (subscription == null)
        {
            throw new ArgumentNullException(nameof(subscription));
        }

        if (versions == null)
        {
            throw new ArgumentNullException(nameof(versions));
        }

        var lambdaProcessor = this.lambdaProcessorFactory.Create<AttachmentLambdaProcessor<TBLLContext>>();

        return lambdaProcessor.Invoke(subscription, versions).ToArray();
    }
}
