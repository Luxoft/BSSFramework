using System.Collections.Immutable;
using System.Security;

using Framework.Application;
using Framework.Database;
using Framework.Database.DALExceptions;

using Anch.SecuritySystem;

namespace Framework.Infrastructure.WebApiExceptionExpander;

public record WebApiExceptionExpanderSettings(ImmutableArray<Type> HandledTypes)
{
    public static WebApiExceptionExpanderSettings Default { get; } = new(
    [
        typeof(SecurityException),
        typeof(DALException),
        typeof(StaleDomainObjectStateException),
        typeof(SecuritySystemException),
        typeof(FluentValidation.ValidationException),
        typeof(BusinessLogicException)
    ]);
}
