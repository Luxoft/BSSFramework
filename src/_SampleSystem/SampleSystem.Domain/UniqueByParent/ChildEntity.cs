using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Framework.Restriction;

using SampleSystem.Domain.UniqueByMaster;

namespace SampleSystem.Domain.UniqueByParent;

[UniqueGroup(nameof(ChildEntity.Parent))]
public class ChildEntity : AuditPersistentDomainObjectBase
{
    private readonly ParentEntity parent;

    [UniqueElement(nameof(Parent))]
    public virtual ParentEntity Parent => this.parent;
}
