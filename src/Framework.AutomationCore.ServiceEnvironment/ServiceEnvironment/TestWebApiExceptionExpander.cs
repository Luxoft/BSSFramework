using Framework.Exceptions;

namespace Framework.DomainDriven.WebApiNetCore;

/// <inheritdoc />
public class TestWebApiExceptionExpander : IWebApiExceptionExpander
{
    private readonly IExceptionExpander exceptionExpander;

    public TestWebApiExceptionExpander(IExceptionExpander exceptionExpander)
    {
        this.exceptionExpander = exceptionExpander ?? throw new ArgumentNullException(nameof(exceptionExpander));
    }

    /// <inheritdoc />
    public Exception Process(Exception baseException)
    {
        var expandedException = this.exceptionExpander.Process(baseException);

        if (expandedException == baseException)
        {
            return baseException;
        }

        return expandedException;
    }
}
