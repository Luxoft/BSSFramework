using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security
{
    public interface IAccessDeniedExceptionServiceContainer<in TPersistentDomainObjectBase> : IAccessDeniedExceptionServiceContainer
        where TPersistentDomainObjectBase : class
    {
        new IAccessDeniedExceptionService<TPersistentDomainObjectBase> AccessDeniedExceptionService
        {
            get;
        }
    }

    public interface IAccessDeniedExceptionServiceContainer
    {
        IAccessDeniedExceptionService AccessDeniedExceptionService
        {
            get;
        }
    }
}
