using Framework.AutomationCore.RootServiceProviderContainer;

using SampleSystem.Domain.EnversBug1676;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

public class EnversBug1676 : TestBase
{
    [Fact]
    public void OneToOneMapping()
    {
        // Arrange
        decimal version1Norm = 1;
        decimal version2Norm = 2;

        var locaitionId = Guid.NewGuid();

        // Act
        this.EvaluateWrite(
                           context =>
                           {
                               var location = new Location1676
                                              {
                                                      Name = "test"
                                              };

                               context.Logics.Location1676.Insert(location, locaitionId);

                               var coefficient = new Coefficient1676() { NormCoefficient = version1Norm };
                               coefficient.Location = location;
                               context.Logics.Default.Create<Coefficient1676>().Save(coefficient);
                           });

        this.EvaluateWrite(
                           context =>
                           {
                               var location = context.Logics.Location1676.GetById(locaitionId);
                               location.Name = location.Name + "NextName";
                               context.Logics.Default.Create<Location1676>().Save(location);
                           });

        this.EvaluateWrite(
                           context =>
                           {
                               var location = context.Logics.Location1676.GetById(locaitionId);
                               location.Name = location.Name + "NextName";
                               location.Coefficient.NormCoefficient = version2Norm;

                               context.Logics.Default.Create<Location1676>().Save(location);
                               context.Logics.Default.Create<Coefficient1676>().Save(location.Coefficient);
                           });

        this.EvaluateWrite(
                           context =>
                           {
                               var location = context.Logics.Location1676.GetById(locaitionId);
                               location.Name = location.Name + "NextName";
                               context.Logics.Default.Create<Location1676>().Save(location);
                           });

        // Assert
        this.EvaluateRead(
                          context =>
                          {
                              var locationRevisions = context.Logics.Location1676.GetObjectRevisions(locaitionId).RevisionInfos.OrderBy(z => z.RevisionNumber).ToList();

                               Assert.Equal(4, locationRevisions.Count);

                              var version1 = context.Logics.Location1676.GetObjectByRevision(locaitionId, locationRevisions[0].RevisionNumber);
                               Assert.Equal(version1Norm, version1.Coefficient.NormCoefficient);

                              var version2 = context.Logics.Location1676.GetObjectByRevision(locaitionId, locationRevisions[1].RevisionNumber);
                               Assert.Equal(version1Norm, version2.Coefficient.NormCoefficient);

                              var version3 = context.Logics.Location1676.GetObjectByRevision(locaitionId, locationRevisions[2].RevisionNumber);
                               Assert.Equal(version2Norm, version3.Coefficient.NormCoefficient);

                              var version4 = context.Logics.Location1676.GetObjectByRevision(locaitionId, locationRevisions[3].RevisionNumber);
                               Assert.Equal(version2Norm, version4.Coefficient.NormCoefficient);
                          });
    }
}
