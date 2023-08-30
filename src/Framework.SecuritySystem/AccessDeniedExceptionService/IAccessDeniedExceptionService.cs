namespace Framework.SecuritySystem.AccessDeniedExceptionService;

public interface IAccessDeniedExceptionService
{
    Exception BuildAccessDeniedException(string message);
}

public interface IAccessDeniedExceptionService<in TPersistentDomainObjectBase> : IAccessDeniedExceptionService
{
    Exception BuildAccessDeniedException<TDomainObject>(
        TDomainObject domainObject,
        IReadOnlyDictionary<string, object> parameters = null)
        where TDomainObject : class, TPersistentDomainObjectBase;
}
