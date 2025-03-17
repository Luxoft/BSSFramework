using Framework.OData;

using SampleSystem.Domain;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class DomainEmployeeTests
{
    [TestMethod]
    public void CreateEmployeeFilter_IsNotVirtual()
    {
        // Arrange

        // Act
        var filter = new TestEmployeeFilter
                     {
                         BusinessUnit = new BusinessUnit()
                     };

        var operation = SelectOperation<Employee>.Default.AddFilter(e => e.CoreBusinessUnit.Id == filter.BusinessUnit.Id);
        var result = operation.IsVirtual;

        // Assert
        result.Should().Be(false);
    }
}
