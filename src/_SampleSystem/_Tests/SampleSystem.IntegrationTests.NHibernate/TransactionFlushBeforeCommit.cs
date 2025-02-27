using FluentAssertions;

using Framework.DomainDriven;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests.IADFRAME_1694;

[TestClass]
public class TransactionFlushBeforeCommit : TestBase
{
    [TestMethod]
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
        this.Evaluate(
                DBSessionMode.Read,
                context => context.Logics.Information.GetUnsecureQueryable().Single(x => x.Email == object1.Email))
            .Name
            .Should()
            .BeEquivalentTo(object2.Name);
    }

    [TestMethod]
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
        this.Evaluate(
                DBSessionMode.Read,
                context => context.Logics.Information.GetUnsecureQueryable().Single(x => x.Id == objectId1))
            .Name
            .Should()
            .BeEquivalentTo(object1.Name);

        this.Evaluate(
                DBSessionMode.Read,
                context => context.Logics.Example1.GetUnsecureQueryable().Single(x => x.Id == objectId2))
            .Field1
            .Should()
            .Be(object2.Field1);
    }
}