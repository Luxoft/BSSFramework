using Framework.Persistent;

namespace Framework.SecuritySystem.ExternalSystem;

public interface ISecurityContextType<out TIdent> : IVisualIdentityObject, IIdentityObject<TIdent>
{
}
