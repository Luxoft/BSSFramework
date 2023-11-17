using FluentAssertions;

using Framework.Authorization.Generated.DTO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class SecurityRoleTests : TestBase
{
    [TestMethod]
    public void GetRootSecurityRole_RootRoleContainsChildrenOperations()
    {
        //Arrange

        //Act

        //Assert
        var rootRole = this.GetAuthControllerEvaluator()
                           .Evaluate(c => c.GetRichBusinessRole(new BusinessRoleIdentityDTO(SampleSystemSecurityRole.TestRootRole.Id)));

        rootRole.SubBusinessRoleLinks.Should().Contain(link => link.SubBusinessRole.Id == SampleSystemSecurityRole.TestChildRole.Id);

        rootRole.BusinessRoleOperationLinks.Should()
                .Contain(link => !link.IsDenormalized && link.Operation.Id == SampleSystemSecurityOperation.EmployeeEdit.Id);

        rootRole.BusinessRoleOperationLinks.Should()
                .Contain(link => !link.IsDenormalized && link.Operation.Id == SampleSystemSecurityOperation.BusinessUnitView.Id);

        rootRole.BusinessRoleOperationLinks.Should()
                .Contain(link => link.IsDenormalized && link.Operation.Id == SampleSystemSecurityOperation.EmployeeView.Id);
    }
}
