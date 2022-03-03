using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Workflow.BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkflowSampleSystem.IntegrationTests.__Support.TestData;

namespace WorkflowSampleSystem.IntegrationTests
{
    [TestClass]
    public class WorkflowAnonimusTypeBuiderTests : TestBase
    {
        private int errorCounter;

        [TestInitialize]
        public void SetUp()
        {
            this.errorCounter = 0;
        }

        [TestMethod]
        public void GetAnonymousType_ConcurrentRequest_ShouldNotFail()
        {
            // Arrange
            var typeMapMembers = new List<ParameterizedTypeMapMember>();
            var typeMap = new TypeMap<ParameterizedTypeMapMember>("name", typeMapMembers);

            var tasks = new Task[2];

            // Act
            for (var i = 0; i < 2; i++)
            {
                tasks[i] = Task.Factory.StartNew(() => this.Act(typeMap));
            }
            Task.WaitAll(tasks);

            // Assert
            this.errorCounter.Should().Be(0);
        }

        private void Act(TypeMap<ParameterizedTypeMapMember> typeMap)
        {
            try
            {
                this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, context =>
                {
                    context.Workflow.AnonymousTypeBuilder.GetAnonymousType(typeMap);
                });
            }
            catch (Exception)
            {
                Interlocked.Increment(ref this.errorCounter);
            }
        }
    }
}
