namespace Framework.SecuritySystem.DiTests;

public class AccessDeniedExceptionServiceTests
{
    [Fact]
    public void CreateNewObject_AccessDeniedMessage_IsValid()
    {
        // Arrange
        var service = new AccessDeniedExceptionService();
        var employee = new Employee();

        // Act
        var result = service.GetAccessDeniedException(
            AccessResult.AccessDeniedResult.Create(employee));

        // Assert
        result.Message.Should().Be($"You have no permissions to create object with type = '{nameof(Employee)}'");
    }

    [Fact]
    public void ChangeExistObject_AccessDeniedMessage_IsValid()
    {
        // Arrange
        var service = new AccessDeniedExceptionService();
        var employee = new Employee() { Id = Guid.NewGuid() };

        // Act
        var result = service.GetAccessDeniedException(
            AccessResult.AccessDeniedResult.Create(employee));

        // Assert
        result.Message.Should().Be($"You have no permissions to access object with type = '{nameof(Employee)}' (id = '{employee.Id}')");
    }

    [Fact]
    public void CreateNewObjectWithOperation_AccessDeniedMessage_IsValid()
    {
        // Arrange
        var service = new AccessDeniedExceptionService();
        var employee = new Employee();
        var securityRule = ExampleSecurityOperation.EmployeeView;

        // Act
        var result = service.GetAccessDeniedException(
            AccessResult.AccessDeniedResult.Create(employee, securityRule));

        // Assert
        result.Message.Should().Be($"You have no permissions to create object with type = '{nameof(Employee)}' (securityRule = '{securityRule.Name}')");
    }

    [Fact]
    public void ChangeExistObjectWithOperation_AccessDeniedMessage_IsValid()
    {
        // Arrange
        var service = new AccessDeniedExceptionService();
        var employee = new Employee() { Id = Guid.NewGuid() };
        var securityRule = ExampleSecurityOperation.EmployeeView;

        // Act
        var result = service.GetAccessDeniedException(
            AccessResult.AccessDeniedResult.Create(employee, securityRule));

        // Assert
        result.Message.Should().Be($"You have no permissions to access object with type = '{nameof(Employee)}' (id = '{employee.Id}', securityRule = '{securityRule.Name}')");
    }
}
