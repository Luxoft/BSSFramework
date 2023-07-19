using Framework.Persistent.Mapping;

using Framework.SecuritySystem;

namespace SampleSystem.Domain.TypedAuth;

[View]
public abstract class TypedAuthPermissionItem<TItem> : PersistentDomainObjectBase
    where TItem : PersistentDomainObjectBase, ISecurityContext
{
    private readonly TypedAuthPermission permission;

    private readonly TItem contextEntity;

    public virtual TypedAuthPermission Permission => this.permission;

    public virtual TItem ContextEntity => this.contextEntity;
}
