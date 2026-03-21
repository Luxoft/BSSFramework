using System.Collections.ObjectModel;

using CommonFramework;

namespace Framework.Validation;

public class AggregateValidationException : ValidationExceptionBase
{
    public AggregateValidationException(IEnumerable<ValidationExceptionBase> innerExceptions)
    {
        if (innerExceptions == null) throw new ArgumentNullException(nameof(innerExceptions));

        this.InnerExceptions = innerExceptions.ToReadOnlyCollection();
    }


    public ReadOnlyCollection<ValidationExceptionBase> InnerExceptions { get; private set; }

    public override string Message
    {
        get { return this.InnerExceptions.Join(Environment.NewLine, x => x.Message); }
    }


    public override string ToString()
    {
        return new AggregateException(this.InnerExceptions).ToString();
    }
}
