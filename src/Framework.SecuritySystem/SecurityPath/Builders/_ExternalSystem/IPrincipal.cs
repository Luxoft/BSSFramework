using Framework.Persistent;

namespace Framework.SecuritySystem.ExternalSystem;

public interface IPrincipal<out TIdent> : IVisualIdentityObject
{
    IEnumerable<IPermission<TIdent>> Permissions { get; }
}
