using CommonFramework.Maybe;

using Framework.Core;

using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class UpdateTests : TestBase
{
    [TestMethod]
    public void ExampleContainer_UpdatePropertyChanged1()
    {
        // Arrange
        var example1Controller = this.GetControllerEvaluator<Example1Controller>();

        var strictSource = new Example1StrictDTO
                           {
                                   Field1 = Guid.NewGuid(),
                                   Field2 = Guid.NewGuid(),
                                   Field3 = Maybe.Return(Guid.NewGuid())
                           };

        var updateDTO = new Example1UpdateDTO(strictSource);

        // Act
        var ident = example1Controller.Evaluate(c => c.UpdateExample1(updateDTO));

        var rich = example1Controller.Evaluate(c => c.GetRichExample1(ident));

        // Assert
        var currentStrictDTO = rich.ToStrict();

        currentStrictDTO.Field1.Should().Be(strictSource.Field1);
        currentStrictDTO.Field2.Should().Be(strictSource.Field2);
        currentStrictDTO.Field3.Should().Be(strictSource.Field3);
    }

    [TestMethod]
    public void ExampleContainer_UpdatePropertyChanged2()
    {
        // Arrange
        var strictSource = new Example1StrictDTO
                           {
                                   Field1 = Guid.NewGuid(),
                                   Field2 = Guid.NewGuid(),
                                   Field3 = Maybe.Return(Guid.NewGuid())
                           };

        var strictTarget = new Example1StrictDTO
                           {
                                   Field1 = strictSource.Field1,
                                   Field2 = Guid.NewGuid(),
                                   Field3 = Maybe.Return(Guid.NewGuid())
                           };


        // Act
        var updateDTO = new Example1UpdateDTO(strictSource, strictTarget);

        // Assert
        updateDTO.Field1.HasValue.Should().Be(false);
        updateDTO.Field2.HasValue.Should().Be(true);
        updateDTO.Field3.HasValue.Should().Be(true);
    }

    [TestMethod]
    public void ExampleContainer_UpdatePropertyChanged3()
    {
        // Arrange
        var example1Controller = this.GetControllerEvaluator<Example1Controller>();

        var ident = example1Controller.Evaluate(c => c.UpdateExample1(new Example1UpdateDTO(new Example1StrictDTO
                                                                          {
                                                                                  Field1 = Guid.NewGuid(),
                                                                                  Field2 = Guid.NewGuid(),
                                                                                  Field3 = Maybe.Return(Guid.NewGuid()),
                                                                                  Items2 = new List<Example2StrictDTO>
                                                                                      {
                                                                                              new Example2StrictDTO { Field1 = Guid.NewGuid(), Field2 = Guid.NewGuid() },
                                                                                              new Example2StrictDTO { Field1 = Guid.NewGuid(), Field2 = Guid.NewGuid() },
                                                                                              new Example2StrictDTO { Field1 = Guid.NewGuid(), Field2 = Guid.NewGuid() }
                                                                                      }
                                                                          })));

        var richObj = example1Controller.Evaluate(c => c.GetRichExample1(ident));

        // Act
        var baseStrictDTO = richObj.ToStrict();

        richObj.Field1 = Guid.NewGuid();

        var removingItem = richObj.Items2[0];
        var resavingItem = richObj.Items2[1];
        var creatingItem = new Example2RichDTO { Field1 = Guid.NewGuid(), Field2 = Guid.NewGuid() };

        richObj.Items2.Remove(removingItem);
        richObj.Items2.Add(creatingItem);
        resavingItem.Field1 = Guid.NewGuid();

        var currentStrictDTO = richObj.ToStrict();

        var updateDTO = new Example1UpdateDTO(currentStrictDTO, baseStrictDTO);

        var removedItems = updateDTO.Items2.OfType<Framework.Persistent.RemoveItemData<Example2UpdateDTO, Example2IdentityDTO>>().ToList();
        var savedItems = updateDTO.Items2.OfType<Framework.Persistent.SaveItemData<Example2UpdateDTO, Example2IdentityDTO>>().ToList();

        var resavedItems = savedItems.Where(item => !item.Value.Id.IsDefault()).ToList();
        var createdItems = savedItems.Where(item => item.Value.Id.IsDefault()).ToList();

        // Assert
        updateDTO.Field1.HasValue.Should().Be(true);
        updateDTO.Field2.HasValue.Should().Be(false);
        updateDTO.Field3.HasValue.Should().Be(false);

        updateDTO.Items2.Count.Should().Be(3);

        removedItems.Count.Should().Be(1);
        removedItems[0].Identity.Should().BeEquivalentTo(removingItem.Identity);

        resavedItems.Count.Should().Be(1);
        resavedItems[0].Value.Identity.Should().BeEquivalentTo(resavingItem.Identity);
        resavedItems[0].Value.Field1.HasValue.Should().Be(true);
        resavedItems[0].Value.Field2.HasValue.Should().Be(false);

        createdItems.Count.Should().Be(1);
        createdItems[0].Value.Identity.Should().BeEquivalentTo(creatingItem.Identity);
        createdItems[0].Value.Field1.HasValue.Should().Be(true);
        createdItems[0].Value.Field2.HasValue.Should().Be(true);
    }
}
