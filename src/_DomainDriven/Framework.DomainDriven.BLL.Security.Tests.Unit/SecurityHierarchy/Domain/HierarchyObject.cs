using Framework.Persistent;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;

public class HierarchyObject : PersistentDomainObjectBase,
                               IDenormalizedHierarchicalPersistentSource<HierarchyObjectAncestorLink, HierarchyObjectToAncestorOrChildLink, HierarchyObject, Guid>
{
    private readonly IList<HierarchyObject> children = new List<HierarchyObject>();

    public HierarchyObject()
    {

    }

    public HierarchyObject(HierarchyObject parent)
    {
        this.Parent = parent;
        this.Parent.children.Add(this);
    }

    public HierarchyObject Parent { get; set; }

    public IEnumerable<HierarchyObject> Children
    {
        get { return this.children; }
    }

    public void RemoveChild(HierarchyObject f)
    {
        this.children.Remove(f);
    }

    public int DeepLevel { get; set; }

    public string Name
    {
        get; set;
    }

    public DateTime? CreateDate { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? ModifyDate { get; set; }

    public string ModifiedBy { get; set; }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        var target = obj as HierarchyObject;
        if (null == target)
        {
            return false;
        }
        return this.Id == target.Id;
    }

    public override string ToString()
    {
        return this.Name;
    }

    public void AddChild(HierarchyObject domainObject)
    {
        this.children.Add(domainObject);
    }

    public void ClearChildren()
    {
        this.children.Clear();
    }
}
