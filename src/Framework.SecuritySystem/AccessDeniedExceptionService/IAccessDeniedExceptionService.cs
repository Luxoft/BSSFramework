namespace Framework.SecuritySystem;

public interface IAccessDeniedExceptionService
{
    Exception GetAccessDeniedException(string message);
}

public interface IAccessDeniedExceptionService<in TPersistentDomainObjectBase> : IAccessDeniedExceptionService
{
    Exception GetAccessDeniedException<TDomainObject>(
            TDomainObject domainObject,
            IReadOnlyDictionary<string, object> extensions = null,
            Func<string, string> formatMessageFunc = null)
            where TDomainObject : class, TPersistentDomainObjectBase;
}
