using Framework.Authorization.Domain;

using NUnit.Framework;

namespace Framework.Authorization.BLL.Tests.Unit;

[TestFixture]
public class BusinessRoleBLLTests : TestBase
{
    [Test]
    public void GetParents_Hierarchic_ReturnRootAndParentAndChild1()
    {
        // Arrange
        BusinessRole child1;
        var parent = this.InitBusinessRoleHierarchic(out child1);
        var root = new BusinessRole();
        new SubBusinessRoleLink(root, parent);
        this.RegisterDomainObject(root);

        // Act
        var r = this.Context.Logics.BusinessRole.GetParents(new[] { child1 }).ToList();

        // Assert
        CollectionAssert.AreEquivalent(new[] { root, child1, parent }, r);
    }

    [Test]
    public void GetParents_Hierarchic_ReturnParentAndChild1()
    {
        // Arrange
        BusinessRole child1;
        var parent = this.InitBusinessRoleHierarchic(out child1);

        // Act
        var r = this.Context.Logics.BusinessRole.GetParents(new[] { child1 }).ToList();

        // Assert
        CollectionAssert.AreEquivalent(new[] { child1, parent }, r);
    }

    private BusinessRole InitBusinessRoleHierarchic(out BusinessRole child1)
    {
        var parent = new BusinessRole();
        child1 = new BusinessRole();
        var child2 = new BusinessRole();
        new SubBusinessRoleLink(parent, child1);
        new SubBusinessRoleLink(parent, child2);

        this.RegisterDomainObject(parent, child1, child2);
        return parent;
    }

    [Test]
    public void GetParents_Hierarchic_ReturnParent()
    {
        // Arrange
        BusinessRole child1;
        var parent = this.InitBusinessRoleHierarchic(out child1);


        // Act
        var r = this.Context.Logics.BusinessRole.GetParents(new[] { parent }).ToList();

        // Assert
        CollectionAssert.AreEquivalent(new[] { parent }, r);
    }
}
