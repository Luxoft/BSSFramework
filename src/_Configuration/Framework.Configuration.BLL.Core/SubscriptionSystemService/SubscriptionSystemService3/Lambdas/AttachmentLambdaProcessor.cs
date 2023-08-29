using Framework.Configuration.Core;
using Framework.Configuration.Domain;



using NativeAttachment = System.Net.Mail.Attachment;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;

/// <summary>
/// Процессор лямбда-выражения типа "Attachment".
/// </summary>
/// <seealso cref="LambdaProcessor" />
public class AttachmentLambdaProcessor<TBLLContext> : LambdaProcessor<TBLLContext>
        where TBLLContext : class
{
    public AttachmentLambdaProcessor(TBLLContext bllContext)
            : base(bllContext)
    {
    }

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
            return Enumerable.Empty<NativeAttachment>();
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

        return Enumerable.Empty<NativeAttachment>();
    }
}
