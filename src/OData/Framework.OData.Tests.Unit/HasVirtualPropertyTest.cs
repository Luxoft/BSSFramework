using System.Linq.Expressions;
using Framework.OData.Tests.Unit.DomainModel;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Framework.OData.Tests.Unit;

[TestFixture]
public class HasVirtualPropertyTest
{
    [Test]
    public void HasVirtualProperty_False()
    {
        var filterModel = new FilterModel();

        Expression<Func<Employee, bool>> expression = z => z.Department.Location == filterModel.Location;

        ClassicAssert.IsFalse(expression.HasVirtualProperty());
    }

    [Test]
    public void HasVirtualPropertyFromInlineType_True()
    {
        Expression<Func<Employee, bool>> expression = z => z.NameEng.FullName == "fff" && z.Department.Name == "sdf";

        ClassicAssert.IsTrue(expression.HasVirtualProperty());
    }

    [Test]
    public void HasVirtualPropertyFromInlineType2_True()
    {
        Expression<Func<Employee, bool>> expression = z => z.Department.Name == "sdf" && z.NameEng.FullName == "fff";

        ClassicAssert.IsTrue(expression.HasVirtualProperty());
    }


    [Test]
    public void HasVirtualPropertyFromInlineType_False()
    {
        Expression<Func<Employee, bool>> expression = z => z.NameEng.FirstName == "fff";

        ClassicAssert.IsFalse(expression.HasVirtualProperty());
    }

    [Test]
    public void HasVirtualPropertyFromVirtual_True()
    {
        Expression<Func<Employee, bool>> expression = z => z.Location == new Location();

        ClassicAssert.IsTrue(expression.HasVirtualProperty());
    }

    [Test]
    public void HasVirtualProperty_True()
    {
        Expression<Func<Employee, bool>> expression = z => z.VirtualProperty == 5;

        ClassicAssert.IsTrue(expression.HasVirtualProperty());
    }

    [Test]
    public void HasVirtualProperty_SubEmployee_True()
    {
        Expression<Func<SubEmployee, bool>> expression = z => z.VirtualProperty == 5;

        ClassicAssert.IsTrue(expression.HasVirtualProperty());
    }

    [Test]
    public void HasNotVirtualProperty_True()
    {
        Expression<Func<Employee, bool>> expression = z => z.NonVirtualProperty == 5;

        ClassicAssert.IsFalse(expression.HasVirtualProperty());
    }
}
