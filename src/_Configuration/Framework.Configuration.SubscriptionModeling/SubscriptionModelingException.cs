using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Framework.Configuration.SubscriptionModeling;

/// <summary>
/// Исключение, бросаемое системой обработки подписок и нотификаций.
/// </summary>
/// <seealso cref="System.Exception" />
[Serializable]
public class SubscriptionModelingException : Exception
{
    /// <summary>
    /// Создаёт новый экземпляр класса <see cref="SubscriptionModelingException"/>.
    /// </summary>
    public SubscriptionModelingException()
    {
    }

    /// <summary>
    /// Создаёт экземпляр класса <see cref="SubscriptionModelingException"/>, используя указанное сообщение об ошибке.
    /// </summary>
    /// <param name="message">Сообщение, описывающее ошибку.</param>
    public SubscriptionModelingException(string message)
            : base(message)
    {
    }

    /// <summary>
    /// Создаёт экземпляр класса <see cref="SubscriptionModelingException"/> с указанным сообщением об ошибке и
    /// ссылкой на внутреннее исключение, которое стало причиной данного исключения.
    /// </summary>
    /// <param name="message">Сообщение, описывающее ошибку.</param>
    /// <param name="innerException">
    ///     Исключение, вызвавшее текущее исключение, или ссылка null, если внутреннее исключение не задано.
    /// </param>
    public SubscriptionModelingException(string message, Exception innerException)
            : base(message, innerException)
    {
    }

    /// <summary>
    /// Создаёт экземпляр класса <see cref="SubscriptionModelingException"/> с сериализованными данными.
    /// </summary>
    /// <param name="info">
    ///     Объект <see cref="T:System.Runtime.Serialization.SerializationInfo" />,
    ///     содержащий сериализованные данные объекта о выбрасываемом исключении.
    /// </param>
    /// <param name="context">
    ///     Объект <see cref="T:System.Runtime.Serialization.StreamingContext" />,
    ///     содержащий контекстные сведения об источнике или назначении.
    /// </param>
    protected SubscriptionModelingException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
    {
    }
}
