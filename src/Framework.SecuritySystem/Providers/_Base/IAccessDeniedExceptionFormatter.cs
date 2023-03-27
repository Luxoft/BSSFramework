using System;

namespace Framework.SecuritySystem;

public interface IAccessDeniedExceptionFormatter
{
    Exception GetAccessDeniedException<TDomainObject>(TDomainObject domainObject, Func<string, string> formatMessageFunc = null);
}
