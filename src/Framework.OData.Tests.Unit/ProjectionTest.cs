using Framework.Persistent;
using Framework.QueryLanguage;

using NUnit.Framework;

namespace Framework.OData.Tests.Unit;

[TestFixture]
public class ProjectionTest
{
    [Test]
    [Ignore("Used for local hand running")]
    public void TestExpandC()
    {
        var employeeLocation = new TestLocation
                               {
                                       Id = Guid.NewGuid(),
                                       Children = new List<TestLocation>()
                               };

        var rootLocation = new TestLocation
                           {
                                   Id = Guid.NewGuid(),
                                   Children = new [] { employeeLocation }
                           };

        employeeLocation.Parent = rootLocation;

        var testEmployee = new TestEmployee
                           {
                                   Id = Guid.NewGuid(),
                                   Name = "testEmployee",
                                   Location = employeeLocation
                           };

        var stream = new[] { testEmployee }.AsQueryable();

        //---------------------------------------------------

        var request = $"$filter=expandC('All', guid'{rootLocation.Id:D}', Location)";

        var selectOperation = Framework.OData.SelectOperation.Parse(request);

        var selectOperationBuilder = new StandartExpressionBuilder();

        var selectOperationGeneric = selectOperationBuilder.ToTyped<TestEmployee, ITestEmployee>(selectOperation);

        //---------------------------------------------------

        var res = selectOperationGeneric.Process(stream).ToList();

        return;
    }
}


public class TestEmployee : ITestEmployee
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public TestLocation Location { get; set; }


    ITestLocation ILocationObject<ITestLocation>.Location
    {
        get { return this.Location; }
    }
}

public class TestLocation : IHierarchicalSource<TestLocation>, ITestLocation
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public TestDepartment Department { get; set; }

    public TestLocation Parent { get; set; }

    public IEnumerable<TestLocation> Children { get; set; }


    ITestDepartment ITestLocation.Department
    {
        get
        {
            return this.Department;
        }
    }

    ITestLocation IParentSource<ITestLocation>.Parent
    {
        get { return this.Parent; }
    }

    IEnumerable<ITestLocation> IChildrenSource<ITestLocation>.Children
    {
        get { return this.Children; }
    }
}

public class TestDepartment : ITestDepartment
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}



public interface ITestEmployee : IDefaultIdentityObject, IVisualIdentityObject, ILocationObject<ITestLocation>
{

}

public interface ITestLocation : IDefaultIdentityObject, IVisualIdentityObject, IHierarchicalSource<ITestLocation>, ILocation
{
    ITestDepartment Department { get; }
}

public interface ITestDepartment : IDefaultIdentityObject, IVisualIdentityObject
{

}
