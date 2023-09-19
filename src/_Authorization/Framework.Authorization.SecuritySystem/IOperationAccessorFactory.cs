using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface IOperationAccessorFactory
{
    IOperationAccessor Create(bool withRunAs);
}
