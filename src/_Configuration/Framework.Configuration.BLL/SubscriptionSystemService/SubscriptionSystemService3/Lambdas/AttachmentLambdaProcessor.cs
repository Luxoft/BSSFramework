using Framework.Configuration.Domain;
using Framework.Subscriptions.Domain;

using NativeAttachment = System.Net.Mail.Attachment;

namespace Framework.Configuration.BLL.SubscriptionSystemService.SubscriptionSystemService3.Lambdas;

/// <summary>
/// Процессор лямбда-выражения типа "Attachment".
/// </summary>
/// <seealso cref="LambdaProcessor" />
public class AttachmentLambdaProcessor<TBLLContext>(TBLLContext bllContext) : LambdaProcessor<TBLLContext>(bllContext)
    where TBLLContext : class
{
    protected override string LambdaName => "Attachment";

    public virtual IEnumerable<NativeAttachment> Invoke<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
    {
        if (versions == null)
        {
            throw new ArgumentNullException(nameof(versions));
        }

        var lambda = subscription.Attachment;

        if (!DomainObjectCompliesLambdaRequiredMode(lambda, versions))
        {
            return [];
        }

        var result = this.TryInvoke(subscription, versions, this.InvokeWithTypedContext);

        return result;
    }

    /// <summary>Исполняет лямбда-выражение с типизированным контекстом бизнес-логики.</summary>
    /// <typeparam name="T">Тип доменного объекта.</typeparam>
    /// <param name="subscription">Подписка.</param>
    /// <param name="versions">Версии доменного объекта.</param>
    /// <returns>Результат вызова лямбда-выражения.</returns>
    protected IEnumerable<NativeAttachment> InvokeWithTypedContext<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
    {
        var funcValue = subscription.Attachment?.FuncValue;

        if (funcValue != null)
        {
            return this.TryCast<IEnumerable<NativeAttachment>>(funcValue(this.BllContext, versions));
        }

        return [];
    }
}
