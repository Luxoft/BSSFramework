using FluentAssertions;

using Framework.HierarchicalExpand;
using Framework.QueryableSource;
using Framework.SecuritySystem.DependencyInjection;
using Framework.SecuritySystem.Rules.Builders;
using V1 = Framework.SecuritySystem.Rules.Builders.MaterializedPermissions;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Xunit;

namespace Framework.SecuritySystem.DiTests;

public class AccessDeniedExceptionServiceTests
{
    [Fact]
    public void CreateNewObject_AccessDeniedMessage_IsValid()
    {
        // Arrange
        var service = new AccessDeniedExceptionService<Guid>();
        var employee = new Employee();

        // Act
        var result = service.GetAccessDeniedException(
            new AccessResult.AccessDeniedResult { DomainObjectInfo = (employee, typeof(Employee)) });

        // Assert
        result.Message.Should().Be($"You have no permissions to create object with type = '{nameof(Employee)}'");
    }

    [Fact]
    public void ChangeExistObject_AccessDeniedMessage_IsValid()
    {
        // Arrange
        var service = new AccessDeniedExceptionService<Guid>();
        var employee = new Employee() { Id = Guid.NewGuid() };

        // Act
        var result = service.GetAccessDeniedException(
            new AccessResult.AccessDeniedResult { DomainObjectInfo = (employee, typeof(Employee)) });

        // Assert
        result.Message.Should().Be($"You have no permissions to access object with type = '{nameof(Employee)}' (id = '{employee.Id}')");
    }

    [Fact]
    public void CreateNewObjectWithOperation_AccessDeniedMessage_IsValid()
    {
        // Arrange
        var service = new AccessDeniedExceptionService<Guid>();
        var employee = new Employee();
        var securityOperation = ExampleSecurityOperation.EmployeeView;

        // Act
        var result = service.GetAccessDeniedException(
            new AccessResult.AccessDeniedResult { DomainObjectInfo = (employee, typeof(Employee)), SecurityOperation = securityOperation });

        // Assert
        result.Message.Should().Be($"You have no permissions to create object with type = '{nameof(Employee)}' (securityOperation = '{securityOperation.Name}')");
    }

    [Fact]
    public void ChangeExistObjectWithOperation_AccessDeniedMessage_IsValid()
    {
        // Arrange
        var service = new AccessDeniedExceptionService<Guid>();
        var employee = new Employee() { Id = Guid.NewGuid() };
        var securityOperation = ExampleSecurityOperation.EmployeeView;

        // Act
        var result = service.GetAccessDeniedException(
            new AccessResult.AccessDeniedResult { DomainObjectInfo = (employee, typeof(Employee)), SecurityOperation = securityOperation });

        // Assert
        result.Message.Should().Be($"You have no permissions to access object with type = '{nameof(Employee)}' (id = '{employee.Id}', securityOperation = '{securityOperation.Name}')");
    }
}
