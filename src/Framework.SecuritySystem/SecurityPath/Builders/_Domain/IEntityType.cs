using Framework.Persistent;

namespace Framework.SecuritySystem;

public interface IEntityType<out TIdent> : IVisualIdentityObject, IIdentityObject<TIdent>
{

}
