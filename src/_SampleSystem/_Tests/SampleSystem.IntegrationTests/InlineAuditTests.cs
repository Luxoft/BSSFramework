using Anch.SecuritySystem.UserSource;
using Anch.Testing.Xunit;

using Framework.Application.Repository;
using Framework.Database;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;
using SampleSystem.Domain.Employee;
using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests;

public abstract class InlineAuditTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [AnchFact]
    public async Task SaveAsync_SetsInlineAuditCreatedByToCurrentUser(CancellationToken ct)
    {
        // Arrange

        // Act
        var (createdByEmployeeId, currentUserId) = await
                                      this.EvaluateAsync(
                                          DBSessionMode.Write,
                                          null,
                                          async ctx =>
                                          {
                                              var rep = ctx.ServiceProvider.GetRequiredService<IRepositoryFactory<ExtendedInlineAuditObj>>().Create();

                                              var currentUserSource = ctx.ServiceProvider.GetRequiredService<ICurrentUserSource<Employee>>();

                                              var obj = new ExtendedInlineAuditObj();

                                              await rep.SaveAsync(obj, ct);

                                              return (obj.CreatedByEmployee?.Id, currentUserSource.CurrentUser.Id);
                                          },
                                          ct);

        // Assert
        Assert.Equal(createdByEmployeeId, currentUserId);
    }
}
