using Anch.Core;

using Framework.BLL.DTOMapping.MergeItemData;
using Framework.Core;

using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

public class UpdateTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
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

        Assert.Equal(strictSource.Field1, currentStrictDTO.Field1);
        Assert.Equal(strictSource.Field2, currentStrictDTO.Field2);
        Assert.Equal(strictSource.Field3, currentStrictDTO.Field3);
    }

    [Fact]
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
        Assert.False(updateDTO.Field1.HasValue);
        Assert.True(updateDTO.Field2.HasValue);
        Assert.True(updateDTO.Field3.HasValue);
    }

    [Fact]
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
                                                                                              new() { Field1 = Guid.NewGuid(), Field2 = Guid.NewGuid() },
                                                                                              new() { Field1 = Guid.NewGuid(), Field2 = Guid.NewGuid() },
                                                                                              new() { Field1 = Guid.NewGuid(), Field2 = Guid.NewGuid() }
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

        var removedItems = updateDTO.Items2.OfType<RemoveItemData<Example2UpdateDTO, Example2IdentityDTO>>().ToList();
        var savedItems = updateDTO.Items2.OfType<SaveItemData<Example2UpdateDTO, Example2IdentityDTO>>().ToList();

        var resavedItems = savedItems.Where(item => !item.Value.Id.IsDefault()).ToList();
        var createdItems = savedItems.Where(item => item.Value.Id.IsDefault()).ToList();

        // Assert
        Assert.True(updateDTO.Field1.HasValue);
        Assert.False(updateDTO.Field2.HasValue);
        Assert.False(updateDTO.Field3.HasValue);

        Assert.Equal(3, updateDTO.Items2.Count);

        Assert.Single(removedItems);
        Assert.Equal(removingItem.Identity, removedItems[0].Identity);

        Assert.Single(resavedItems);
        Assert.Equal(resavingItem.Identity, resavedItems[0].Value.Identity);
        Assert.True(resavedItems[0].Value.Field1.HasValue);
        Assert.False(resavedItems[0].Value.Field2.HasValue);

        Assert.Single(createdItems);
        Assert.Equal(creatingItem.Identity, createdItems[0].Value.Identity);
        Assert.True(createdItems[0].Value.Field1.HasValue);
        Assert.True(createdItems[0].Value.Field2.HasValue);
    }
}
