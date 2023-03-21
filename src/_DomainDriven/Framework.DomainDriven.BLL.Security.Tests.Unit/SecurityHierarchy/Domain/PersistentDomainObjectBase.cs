using System;
using Framework.Persistent;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;

public class PersistentDomainObjectBase : DomainObjectBase, IIdentityObject<Guid>
{
    private Guid _id = Guid.NewGuid();

    public Guid Id { get { return this._id; } }
    public bool Active { get; private set; }
}
