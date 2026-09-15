using Anch.Testing.Xunit;

using Framework.AutomationCore.RootServiceProviderContainer;

using Microsoft.Data.SqlClient;

using SampleSystem.Domain.Directories;
using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests;

public abstract class EnversTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [AnchFact]
    public async Task FullLifecycle_CreateUpdatePrimitiveUpdateReferenceRemove_AuditRowsMatchNHibernate(CancellationToken ct)
    {
        // Arrange
        var countryA = this.DataManager.SaveCountry();
        var countryB = this.DataManager.SaveCountry();

        var locationId = Guid.NewGuid();

        // Act

        this.EvaluateWrite(
                           context =>
                           {
                               var location = new Location
                               {
                                   Name = "InitialName",
                                   Country = context.Logics.Country.GetById(countryA.Id),
                                   Code = 1,
                                   CloseDate = 10,
                               };

                               context.Logics.Location.Insert(location, locationId);
                           });

        this.EvaluateWrite(
                           context =>
                           {
                               var location = context.Logics.Location.GetById(locationId, true)!;
                               location.Name = "UpdatedName";
                               context.Logics.Location.Save(location);
                           });

        this.EvaluateWrite(
                           context =>
                           {
                               var location = context.Logics.Location.GetById(locationId, true)!;
                               location.Country = context.Logics.Country.GetById(countryB.Id);
                               context.Logics.Location.Save(location);
                           });

        this.EvaluateWrite(
                           context =>
                           {
                               var location = context.Logics.Location.GetById(locationId, true)!;
                               context.Logics.Location.Remove(location);
                           });

        // Assert
        await using var connection = new SqlConnection(this.ActualConnectionString.Value);
        await connection.OpenAsync(ct);

        await using var command = connection.CreateCommand();
        command.CommandText = """
                              SELECT REV, REVTYPE, Name, countryId
                              FROM appAudit.LocationAudit
                              WHERE Id = @id
                              ORDER BY REV
                              """;
        command.Parameters.AddWithValue("@id", locationId);

        var revisions = new List<(short RevType, string Name, Guid? CountryId)>();

        await using var reader = await command.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            revisions.Add((
                              reader.GetInt16(1),
                              reader.GetString(2),
                              reader.IsDBNull(3) ? null : reader.GetGuid(3)));
        }

        Assert.Equal(4, revisions.Count);

        Assert.Equal((short)0, revisions[0].RevType); // Added
        Assert.Equal("InitialName", revisions[0].Name);
        Assert.Equal(countryA.Id, revisions[0].CountryId);

        Assert.Equal((short)1, revisions[1].RevType); // Modified: primitive field
        Assert.Equal("UpdatedName", revisions[1].Name);
        Assert.Equal(countryA.Id, revisions[1].CountryId);

        Assert.Equal((short)1, revisions[2].RevType); // Modified: reference field
        Assert.Equal("UpdatedName", revisions[2].Name);
        Assert.Equal(countryB.Id, revisions[2].CountryId);

        Assert.Equal((short)2, revisions[3].RevType); // Deleted
        Assert.Equal("UpdatedName", revisions[3].Name);
        Assert.Equal(countryB.Id, revisions[3].CountryId);
    }
}
