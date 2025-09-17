using CommonFramework;
using CommonFramework.ExpressionComparers;

using Framework.Core;

using NUnit.Framework;
using FluentAssertions;

using Framework.Core.ExpressionComparers;

namespace Framework.OData.Tests.Unit;

[TestFixture]
public class ODataVirtualPropTests
{
    [Test]
    public void CreateEmployeeFilter_IsVirtual()
    {
        // Arrange
        var filter = new TestEmployeeFilter
                     {
                             BusinessUnit = new BusinessUnit()
                     };

        var operation = SelectOperation<Employee>.Default.OverrideFilter(e => e.VirtualBusinessUnit.Id == filter.BusinessUnit.Id);

        // Act
        var result = operation.IsVirtual;

        // Assert
        result.Should().Be(true);
    }


    [Test]
    public void CreateEmployeeFilterDeep_IsVirtual()
    {
        // Arrange
        var filter = new TestEmployeeFilter
                     {
                             BusinessUnit = new BusinessUnit()
                     };

        var operation = SelectOperation<Employee>.Default.OverrideFilter(e => e.CoreBusinessUnit.VirtualId == filter.BusinessUnit.Id);

        // Act
        var result = operation.IsVirtual;

        // Assert
        result.Should().Be(true);
    }

    [Test]
    public void CreateEmployeeFilter_IsNotVirtual()
    {
        // Arrange
        var filter = new TestEmployeeFilter
                     {
                             BusinessUnit = new BusinessUnit()
                     };

        var operation = SelectOperation<Employee>.Default.OverrideFilter(e => e.CoreBusinessUnit.Id == filter.BusinessUnit.Id);

        // Act
        var result = operation.IsVirtual;

        // Assert
        result.Should().Be(false);
    }

    [Test]
    public void CreateMixedEmployeeFilter1_RealFilterPartialEquals()
    {
        // Arrange
        var filter = new TestEmployeeFilter
                     {
                             BusinessUnit = new BusinessUnit()
                     };

        var filterExpr1 = ExpressionHelper.Create((Employee e) => e.CoreBusinessUnit.Id == filter.BusinessUnit.Id);
        var filterExpr2 = ExpressionHelper.Create((Employee e) => e.VirtualBusinessUnit.Id == filter.BusinessUnit.Id);

        var operation = SelectOperation<Employee>.Default.OverrideFilter(filterExpr1.BuildAnd(filterExpr2));

        // Act
        var realFilter = operation.Filter.ToRealFilter().Optimize();

        // Assert
        var equals = ExpressionComparer.Value.Equals(realFilter, filterExpr1);
        equals.Should().Be(true);
    }


    [Test]
    public void CreateMixedEmployeeFilter2_RealFilterPartialEquals()
    {
        // Arrange
        var filter = new TestEmployeeFilter
                     {
                             BusinessUnit = new BusinessUnit()
                     };

        var filterExpr1 = ExpressionHelper.Create((Employee e) => e.CoreBusinessUnit.Id == filter.BusinessUnit.Id);
        var filterExpr2 = ExpressionHelper.Create((Employee e) => e.CoreBusinessUnit.VirtualId == filter.BusinessUnit.Id);

        var operation = SelectOperation<Employee>.Default.OverrideFilter(filterExpr1.BuildAnd(filterExpr2));

        // Act
        var realFilter = operation.Filter.ToRealFilter().Optimize();

        // Assert
        var equals = ExpressionComparer.Value.Equals(realFilter, filterExpr1);
        equals.Should().Be(true);
    }

    [Test]
    public void CreateMixedEmployeeFilter3_RealFilterPartialEquals()
    {
        // Arrange
        var filter = new TestEmployeeFilter
                     {
                             BusinessUnit = new BusinessUnit()
                     };

        var filterExpr1 = ExpressionHelper.Create((Employee e) => e.CoreBusinessUnit.Id == filter.BusinessUnit.Id);
        var filterExpr2 = ExpressionHelper.Create((Employee e) => e.CoreBusinessUnit.Id == e.CoreBusinessUnit.VirtualId);

        var operation = SelectOperation<Employee>.Default.OverrideFilter(filterExpr1.BuildAnd(filterExpr2));

        // Act
        var realFilter = operation.Filter.ToRealFilter().Optimize();

        // Assert
        var equals = ExpressionComparer.Value.Equals(realFilter, filterExpr1);
        equals.Should().Be(true);
    }

    [Test]
    public void CreateMixedEmployeeFilter4_RealFilterPartialEquals()
    {
        // Arrange
        var filter = new TestEmployeeFilter
                     {
                             BusinessUnit = new BusinessUnit()
                     };

        var filterExpr1 = ExpressionHelper.Create((Employee e) => e.CoreBusinessUnit.Id == filter.BusinessUnit.Id);
        var filterExpr2 = ExpressionHelper.Create((Employee e) => e.CoreBusinessUnit.Id == e.CoreBusinessUnit.VirtualId);

        var operation = SelectOperation<Employee>.Default.OverrideFilter(filterExpr1.BuildAnd(filterExpr2));

        // Act
        var realFilter = operation.Filter.ToRealFilter().Optimize();

        // Assert
        var equals = ExpressionComparer.Value.Equals(realFilter, filterExpr1);
        equals.Should().Be(true);
    }

    private class Employee
    {
        private BusinessUnit coreBusinessUnit;

        public BusinessUnit CoreBusinessUnit => this.coreBusinessUnit;


        public BusinessUnit VirtualBusinessUnit { get; set; }
    }

    private class TestEmployeeFilter
    {
        public BusinessUnit BusinessUnit { get; set; }
    }

    private class BusinessUnit
    {
        private Guid id;

        public Guid Id => this.id;


        public Guid VirtualId { get; set; }
    }
}
