using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Lambdas;

using Attachment = System.Net.Mail.Attachment;

namespace Framework.Subscriptions.Services;

public class AttachmentsResolver<TBLLContext>(LambdaProcessorFactory<TBLLContext> lambdaProcessorFactory)
    where TBLLContext : class
{
    public virtual IEnumerable<Attachment> Resolve<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
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

        var lambdaProcessor = lambdaProcessorFactory.Create<AttachmentLambdaProcessor<TBLLContext>>();

        return lambdaProcessor.Invoke(subscription, versions).ToArray();
    }
}
