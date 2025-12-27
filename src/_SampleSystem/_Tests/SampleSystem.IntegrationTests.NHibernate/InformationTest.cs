using SampleSystem.Domain;

using Framework.DomainDriven;

using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class InformationTest : TestBase
{
    [TestMethod]
    public void CreateAndRemoveInformation_ContainsIntegrationEvents()
    {
        // Arrange
        this.ClearIntegrationEvents();

        // Act
        var id = this.Evaluate(DBSessionMode.Write, context =>
                                                    {
                                                        var newObj = new Information() { Name = "ololo" };

                                                        context.Logics.Information.Save(newObj);

                                                        return newObj.Id;
                                                    });

        this.Evaluate(DBSessionMode.Write, context =>
                                           {
                                               var obj = context.Logics.Information.GetById(id, true);

                                               context.Logics.Information.Remove(obj);
                                           });

        // Assert
        this.GetIntegrationEvents<InformationSaveEventDTO>().Should().ContainSingle(dto => dto.Information.Id == id);
        this.GetIntegrationEvents<InformationRemoveEventDTO>().Should().ContainSingle(dto => dto.Information.Id == id);
    }
}
