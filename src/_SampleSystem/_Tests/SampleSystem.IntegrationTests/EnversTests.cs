using Anch.Testing.Xunit;

using Framework.AutomationCore.RootServiceProviderContainer;

using Microsoft.Data.SqlClient;

using SampleSystem.Domain.Directories;
using SampleSystem.Domain.Enums;
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
        Assert.Null(revisions[3].EqualBU);
        Assert.Null(revisions[3].BusinessUnitId);
    }

    [AnchFact]
    public async Task TptHierarchy_CreateUpdateBaseField_BaseAuditRowMatchesLeafAuditRow(CancellationToken ct)
    {
        // Arrange
        var legalEntityId = Guid.NewGuid();

        // Act

        this.EvaluateWrite(
                           context =>
                           {
                               var legalEntity = new CompanyLegalEntity
                                                  {
                                                      Active = true,
                                                      Name = "TptAuditTest",
                                                      NameEnglish = "TptAuditTestEnglish",
                                                      Code = "TptAuditTestCode",
                                                      Type = CompanyLegalEntityType.LegalEntity
                                                  };

                               context.Logics.CompanyLegalEntity.Insert(legalEntity, legalEntityId);
                           });

        this.EvaluateWrite(
                           context =>
                           {
                               var legalEntity = context.Logics.CompanyLegalEntity.GetById(legalEntityId, true)!;
                               legalEntity.NameEnglish = "TptAuditTestEnglishUpdated";
                               context.Logics.CompanyLegalEntity.Save(legalEntity);
                           });

        // Assert
        await using var connection = new SqlConnection(this.ActualConnectionString.Value);
        await connection.OpenAsync(ct);

        await using var leafCommand = connection.CreateCommand();
        leafCommand.CommandText = """
                                  SELECT REV
                                  FROM appAudit.CompanyLegalEntityAudit
                                  WHERE Id = @id
                                  ORDER BY REV
                                  """;
        leafCommand.Parameters.AddWithValue("@id", legalEntityId);

        var leafRevisions = new List<long>();
        await using (var reader = await leafCommand.ExecuteReaderAsync(ct))
        {
            while (await reader.ReadAsync(ct))
            {
                leafRevisions.Add(reader.GetInt64(0));
            }
        }

        await using var baseCommand = connection.CreateCommand();
        baseCommand.CommandText = """
                                  SELECT REV, NameEnglish
                                  FROM appAudit.LegalEntityBaseAudit
                                  WHERE Id = @id
                                  ORDER BY REV
                                  """;
        baseCommand.Parameters.AddWithValue("@id", legalEntityId);

        var baseRevisions = new List<(long Rev, string? NameEnglish)>();
        await using (var reader = await baseCommand.ExecuteReaderAsync(ct))
        {
            while (await reader.ReadAsync(ct))
            {
                baseRevisions.Add((reader.GetInt64(0), reader.IsDBNull(1) ? null : reader.GetString(1)));
            }
        }

        Assert.Equal(2, leafRevisions.Count);

        // Every leaf-type (CompanyLegalEntityAudit) revision must have a matching row in the base-type (LegalEntityBaseAudit) table sharing the same (Id, REV).
        Assert.Equal(leafRevisions, baseRevisions.Select(z => z.Rev));

        Assert.Equal("TptAuditTestEnglish", baseRevisions[0].NameEnglish);
        Assert.Equal("TptAuditTestEnglishUpdated", baseRevisions[1].NameEnglish);
    }
}
