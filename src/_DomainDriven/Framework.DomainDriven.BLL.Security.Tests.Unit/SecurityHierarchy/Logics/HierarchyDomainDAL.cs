using CommonFramework;

using Framework.Core;
using Framework.DomainDriven.UnitTest.Mock;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;

public class HierarchyDomainDAL : MockDAL<HierarchyObject, Guid>
{
    public HierarchyDomainDAL(IList<HierarchyObject> collection) : base(collection)
    {
    }

    public override void Remove(HierarchyObject domainObject)
    {
        base.Remove(domainObject);
        this.Actions.Add(q => q.Where(z => z.Parent == domainObject).Foreach(z => z.Parent = null));
    }

    public override void Save(HierarchyObject domainObject)
    {
        base.Save(domainObject);
        this.Actions.Add(z => domainObject.Parent.Maybe(q => q.AddChild(domainObject)));
    }
}
