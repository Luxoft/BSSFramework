using System.Collections.ObjectModel;
using System.Diagnostics;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Validation;

public class ValidationResult
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Lazy<ReadOnlyCollection<ValidationExceptionBase>> _lazyErrors;

    internal ValidationResult(IEnumerable<ValidationExceptionBase> errors)
    {
        if (errors == null) throw new ArgumentNullException(nameof(errors));

        this._lazyErrors = LazyHelper.Create(() => errors.ToReadOnlyCollection());
    }

    public ReadOnlyCollection<ValidationExceptionBase> Errors
    {
        get { return this._lazyErrors.Value; }
    }

    public bool HasErrors
    {
        get { return this.Errors.Any(); }
    }

    public void TryThrow()
    {
        this.Errors.Match(
                          () => { },
                          ex => { throw ex; },
                          exceptions => { throw new AggregateValidationException(exceptions); });
    }

    public override string ToString()
    {
        return this.Errors.Select(z => z.Message).Join(Environment.NewLine);
    }

    public static ValidationResult FromCondition(bool isSuccess, Func<string> getErrorMessage)
    {
        if (getErrorMessage == null) throw new ArgumentNullException(nameof(getErrorMessage));

        return FromCondition(isSuccess, () => new ValidationException(getErrorMessage()));
    }

    public static ValidationResult FromMaybe(Maybe<string> maybeErrorMessage)
    {
        return maybeErrorMessage.Match(str => ValidationResult.CreateError(new ValidationException(str)),
                                       () => ValidationResult.Success);
    }

    public static ValidationResult FromCondition(bool isSuccess, Func<ValidationExceptionBase> getError)
    {
        if (getError == null) throw new ArgumentNullException(nameof(getError));

        return isSuccess ? ValidationResult.Success
                       : ValidationResult.CreateError(getError());
    }

    public static ValidationResult TryCatch(Action action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        try
        {
            action();
            return Success;
        }
        catch (Exception ex)
        {
            return CreateError(ex.Message);
        }
    }

    [StringFormatMethod("format")]
    public static ValidationResult CreateError(string format, params object[] args)
    {
        return CreateError(string.Format(format, args));
    }

    public static ValidationResult CreateError(string message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));

        return new ValidationResult(new[] { new ValidationException(message) });
    }

    internal static ValidationResult CreateError(object exceptionObject)
    {
        return (exceptionObject as string).ToMaybe().Select(CreateError)
                                          .Or(() => (exceptionObject as ValidationExceptionBase).ToMaybe().Select(CreateError))
                                          .Or(() => (exceptionObject as Exception).ToMaybe().Select(error => CreateError(new ValidationException(error.Message, error))))
                                          .GetValue(() => new Exception("Invalid exceptionObject"));
    }

    public static ValidationResult CreateError(ValidationExceptionBase exception)
    {
        if (exception == null) throw new ArgumentNullException(nameof(exception));

        return new ValidationResult(new[] { exception });
    }

    public static readonly ValidationResult Success = new ValidationResult(new ValidationExceptionBase[0].ToReadOnlyCollection());

    public static ValidationResult operator +(ValidationResult result1, ValidationResult result2)
    {
        if (result1 == null) throw new ArgumentNullException(nameof(result1));
        if (result2 == null) throw new ArgumentNullException(nameof(result2));

        if (!result1.HasErrors)
        {
            return result2;
        }
        else if (!result2.HasErrors)
        {
            return result1;
        }
        else
        {
            return new ValidationResult(result1.Errors.Union(result2.Errors));
        }
    }

    internal static Func<T, ValidationResult> GetSuccessFunc<T>()
    {
        return _ => ValidationResult.Success;
    }
}
