using System.Collections.ObjectModel;

using CommonFramework;

// ReSharper disable once CheckNamespace
namespace Framework.Validation;

public class AggregateValidationException : ValidationExceptionBase
{
    public AggregateValidationException(IEnumerable<ValidationExceptionBase> innerExceptions)
    {
        if (innerExceptions == null) throw new ArgumentNullException(nameof(innerExceptions));

        this.InnerExceptions = innerExceptions.ToReadOnlyCollection();
    }


    public ReadOnlyCollection<ValidationExceptionBase> InnerExceptions { get; }

    public override string Message => this.InnerExceptions.Join(Environment.NewLine, x => x.Message);

    public override string ToString() => new AggregateException(this.InnerExceptions).ToString();
}
