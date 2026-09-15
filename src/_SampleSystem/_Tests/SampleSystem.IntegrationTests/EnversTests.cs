using Anch.Testing.Xunit;

using Framework.AutomationCore.RootServiceProviderContainer;

using Microsoft.Data.SqlClient;

using SampleSystem.Domain.MU;
using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests;

public abstract class EnversTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [AnchFact]
    public async Task FullLifecycle_CreateUpdatePrimitiveUpdateReferenceRemove_AuditRowsMatchNHibernate(CancellationToken ct)
    {
        // Arrange
        var managementUnit = this.DataManager.SaveManagementUnit();
        var businessUnitA = this.DataManager.SaveBusinessUnit();
        var businessUnitB = this.DataManager.SaveBusinessUnit();

        var linkId = Guid.NewGuid();

        // Act

        this.EvaluateWrite(
                           context =>
                           {
                               var link = new ManagementUnitAndBusinessUnitLink(
                                                                                context.Logics.ManagementUnit.GetById(managementUnit.Id)!,
                                                                                context.Logics.BusinessUnit.GetById(businessUnitA.Id)!)
                               {
                                   EqualBU = false
                               };

                               context.Logics.ManagementUnitAndBusinessUnitLink.Insert(link, linkId);
                           });

        this.EvaluateWrite(
                           context =>
                           {
                               var link = context.Logics.ManagementUnitAndBusinessUnitLink.GetById(linkId, true)!;
                               link.EqualBU = true;
                               context.Logics.ManagementUnitAndBusinessUnitLink.Save(link);
                           });

        this.EvaluateWrite(
                           context =>
                           {
                               var link = context.Logics.ManagementUnitAndBusinessUnitLink.GetById(linkId, true)!;
                               link.BusinessUnit = context.Logics.BusinessUnit.GetById(businessUnitB.Id)!;
                               context.Logics.ManagementUnitAndBusinessUnitLink.Save(link);
                           });

        this.EvaluateWrite(
                           context =>
                           {
                               var link = context.Logics.ManagementUnitAndBusinessUnitLink.GetById(linkId, true)!;
                               context.Logics.ManagementUnitAndBusinessUnitLink.Remove(link);
                           });

        // Assert
        await using var connection = new SqlConnection(this.ActualConnectionString.Value);
        await connection.OpenAsync(ct);

        await using var command = connection.CreateCommand();
        command.CommandText = """
                              SELECT REV, REVTYPE, EqualBU, businessUnitId
                              FROM appAudit.ManagementUnitAndBusinessUnitLinkAudit
                              WHERE Id = @id
                              ORDER BY REV
                              """;
        command.Parameters.AddWithValue("@id", linkId);

        var revisions = new List<(short RevType, bool? EqualBU, Guid? BusinessUnitId)>();

        await using var reader = await command.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            revisions.Add((
                              reader.GetInt16(1),
                              reader.IsDBNull(2) ? null : reader.GetBoolean(2),
                              reader.IsDBNull(3) ? null : reader.GetGuid(3)));
        }

        Assert.Equal(4, revisions.Count);

        Assert.Equal((short)0, revisions[0].RevType); // Added
        Assert.False(revisions[0].EqualBU);
        Assert.Equal(businessUnitA.Id, revisions[0].BusinessUnitId);

        Assert.Equal((short)1, revisions[1].RevType); // Modified: primitive field
        Assert.True(revisions[1].EqualBU);
        Assert.Equal(businessUnitA.Id, revisions[1].BusinessUnitId);

        Assert.Equal((short)1, revisions[2].RevType); // Modified: reference field
        Assert.True(revisions[2].EqualBU);
        Assert.Equal(businessUnitB.Id, revisions[2].BusinessUnitId);

        Assert.Equal((short)2, revisions[3].RevType); // Deleted: NHibernate.Envers is configured with StoreDataAtDelete = false, so no field snapshot is kept on the delete revision
    }
}
