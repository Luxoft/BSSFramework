namespace Framework.Subscriptions;

public class SubscriptionModelingException : Exception
{
    public SubscriptionModelingException()
    {
    }

    public SubscriptionModelingException(string message)
            : base(message)
    {
    }

    public SubscriptionModelingException(string message, Exception innerException)
            : base(message, innerException)
    {
    }
}
