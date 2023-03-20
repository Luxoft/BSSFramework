using Framework.Async;

namespace Framework.ServiceModel.Async;

public interface IAsyncSecurityService<in TIdentity, in TSecurityOperationCode>
{
    IAsyncProcessFunc<TIdentity, TSecurityOperationCode, bool> SecurityFunc { get; }
}
