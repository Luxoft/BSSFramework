namespace Framework.DomainDriven.WebApiNetCore;

/// <summary>
/// IAD Framework Internal Server Exception
/// </summary>
/// <inheritdoc cref="Exception" />
public class InternalServerException(string message, Exception innerException) : Exception(message, innerException)
{
    /// <summary>
    /// Default exception message
    /// </summary>
    public const string DefaultMessage = "Server has encountered system error. Message was sent to the support group";
}
