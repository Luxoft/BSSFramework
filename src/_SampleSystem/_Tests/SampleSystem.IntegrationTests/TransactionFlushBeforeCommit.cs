﻿using System;
using System.Linq;
using FluentAssertions;
using Framework.DomainDriven.BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests.IADFRAME_1694
{
    [TestClass]
    public class TransactionFlushBeforeCommit : TestBase
    {
        [TestMethod]
        public void ShouldNotConsiderChangesWhileTransactionIsNotCommited()
        {
            // Arrange
            var object1 = new Information { Name = "object1", Email = "email@luxoft.fake" };
            this.Environment.GetContextEvaluator().Evaluate(
                DBSessionMode.Write,
                context => context.Logics.Information.Insert(object1, Guid.NewGuid()));

            var object2 = new Information { Name = "object2", Email = object1.Email };

            // Act
            this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
                                                           {
                                                               context.Logics.Information.Insert(object2, Guid.NewGuid());

                                                               var oldObject = context.Logics.Information.GetUnsecureQueryable().Single(x => x.Email == object1.Email);

                                                               context.Logics.Information.Remove(oldObject);
                                                           });

            // Assert
            this.Environment.GetContextEvaluator().Evaluate(
                    DBSessionMode.Read,
                    context => context.Logics.Information.GetUnsecureQueryable().Single(x => x.Email == object1.Email))
                .Name
                .Should().BeEquivalentTo(object2.Name);
        }
    }
}
