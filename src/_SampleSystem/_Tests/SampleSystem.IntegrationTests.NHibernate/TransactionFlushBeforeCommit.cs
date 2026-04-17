using Framework.Application;
using Framework.Database;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.ForUpdate;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

public class TransactionFlushBeforeCommit : TestBase
{
    [Fact]
    public void ShouldNotConsiderChangesWhileTransactionIsNotCommited()
    {
        // Arrange
        var object1 = new Information { Name = "object1", Email = "email@luxoft.fake" };
        this.Evaluate(
            DBSessionMode.Write,
            context => context.Logics.Information.Insert(object1, Guid.NewGuid()));

        var object2 = new Information { Name = "object2", Email = object1.Email };

        // Act
        this.Evaluate(DBSessionMode.Write,
                      context =>
                      {
                          context.Logics.Information.Insert(object2, Guid.NewGuid());

                          var oldObject = context.Logics.Information.GetUnsecureQueryable().Single(x => x.Email == object1.Email);

                          context.Logics.Information.Remove(oldObject);
                      });

        // Assert
        var actualObject2Name = this.Evaluate(
            DBSessionMode.Read,
            context => context.Logics.Information.GetUnsecureQueryable().Single(x => x.Email == object1.Email).Name);

        Assert.Equal(object2.Name, actualObject2Name);
    }

    [Fact]
    public void FewDifferentTypeObjectWithTheSameId_Should_SuccessfullySave()
    {
        // Arrange
        var objectId1 = Guid.NewGuid();
        var objectId2 = objectId1; // NOTE: with different guids test successfully completes
        var object1 = new Information { Name = "object1", Email = "email@luxoft.fake" };
        var object2 = new Example1 { Field1 = Guid.NewGuid() };

        // Act
        this.Evaluate(DBSessionMode.Write,
                      context =>
                      {
                          context.Logics.Information.Insert(object1, objectId1);
                          context.Logics.Example1.Insert(object2, objectId2);
                      });

        // Assert
        var savedInformationName = this.Evaluate(
            DBSessionMode.Read,
            context => context.Logics.Information.GetUnsecureQueryable().Single(x => x.Id == objectId1).Name);

        Assert.Equal(object1.Name, savedInformationName);

        var savedExampleField1 = this.Evaluate(
            DBSessionMode.Read,
            context => context.Logics.Example1.GetUnsecureQueryable().Single(x => x.Id == objectId2).Field1);

        Assert.Equal(object2.Field1, savedExampleField1);
    }
}
