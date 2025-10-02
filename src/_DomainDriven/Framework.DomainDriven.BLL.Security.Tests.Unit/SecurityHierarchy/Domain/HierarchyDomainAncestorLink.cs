// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchyDomainAncestorLink.cs" company="">
//
// </copyright>
// <summary>
//   Defines the HierarchyObjectAncestorLink type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;

public class HierarchyObjectAncestorLink : PersistentDomainObjectBase, IModifiedHierarchicalAncestorLink<HierarchyObject, HierarchyObjectToAncestorOrChildLink, Guid>
{
    public HierarchyObject Ancestor { get; set; }

    public HierarchyObject Child { get; set; }

    public override string ToString()
    {
        return $"{this.Ancestor} {this.Child}";
    }
}
