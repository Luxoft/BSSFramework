using System.Net.Mail;

using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Core;
using Framework.Notification.New;

namespace SampleSystem.BLL;

/// <summary>
///     Класс для рассылки уведомлений по исключениям в SampleSystem.
/// </summary>
/// <seealso cref="Framework.Configuration.BLL.Notification.ExceptionMessageSender" />
public sealed class SampleSystemExceptionMessageSender : ExceptionMessageSender
{
    /// <summary>
    ///     Создаёт экземпляр класса <see cref="SampleSystemExceptionMessageSender" />.
    /// </summary>
    /// <param name="context">Контекст конфигурации.</param>
    /// <param name="messageSender">Оборачиваемый экземпляр класса рассылки уведомлений.</param>
    /// <param name="fromAddress">Адрес отправителя сообщения.</param>
    /// <param name="toAddresses">Адреса получателей сообщения.</param>
    public SampleSystemExceptionMessageSender(
            IConfigurationBLLContext context,
            IMessageSender<Message> messageSender,
            MailAddress fromAddress,
            IEnumerable<string> toAddresses)
            : base(context, messageSender, fromAddress, toAddresses)
    {
    }

    /// <inheritdoc />
    protected override bool SkipSend(Exception exception)
    {
        return false;
    }

    /// <inheritdoc />
    protected override IEnumerable<Type> GetExceptTypes()
    {
        return new[] { typeof(SampleSystemException) };
    }
}
