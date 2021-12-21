using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Framework.DomainDriven.BLL;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests.Workflow
{
    [TestClass]
    public class WorkflowTransitionTests : TestBase
    {
        [TestMethod]
        public void PerformTransition_TransitionVersionShouldNotBeChanged()
        {
            // Arrange
            var transitionVersionBefore = this.Environment.GetContextEvaluator().Evaluate(
                DBSessionMode.Read,
                context => context.Workflow.Logics.Transition.GetById(Guid.Parse("CACA9DB4-9DA6-48AA-9FD3-A311016CB715"), true).Version);

            var locationId = this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                var location = new Location { Name = "location", Code = 1000,  CloseDate = 20 };

                context.Logics.Location.Save(location);

                return location.Id;
            });

            // Act
            this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                var location = context.Logics.Location.GetById(locationId, true);

                var command = this.GetCommand(context, location, "Act");

                context.Workflow.Logics.WorkflowInstance.ExecuteCommand(command);
            });

            // Assert
            var transitionVersionAfter = this.Environment.GetContextEvaluator().Evaluate(
                DBSessionMode.Read,
                context => context.Workflow.Logics.Transition.GetById(Guid.Parse("CACA9DB4-9DA6-48AA-9FD3-A311016CB715"), true).Version);

            transitionVersionAfter.Should().Be(transitionVersionBefore);
        }


        [TestMethod]
        public void RunWorkflowInstancesSimultaneously_ShouldNotFail()
        {
            // Arrange
            var locationIds = new List<Guid>();

            for (var i = 1; i <= 5; i++)
            {
                var locationId = this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
                {
                    var location = new Location { Name = $"location{i}", Code = 1000 + i, CloseDate = 20 };

                    context.Logics.Location.Save(location);

                    return location.Id;
                });

                locationIds.Add(locationId);
            }

            var tasks = new List<System.Threading.Tasks.Task>();
            locationIds.ForEach(locationId =>
            {
                var task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
                    {
                        var location = context.Logics.Location.GetById(locationId, true);

                        var command = this.GetCommand(context, location, "Act");

                        context.Workflow.Logics.WorkflowInstance.ExecuteCommand(command);
                    });
                });
                tasks.Add(task);
            });

            // Act
            var action = new Action(() => System.Threading.Tasks.Task.WaitAll(tasks.ToArray()));

            // Assert
            action.Should().NotThrow();
        }

        private ExecuteCommandRequest GetCommand<TDomain>(ISampleSystemBLLContext context, TDomain workflowParameter, string commandName)
            where TDomain : Domain.PersistentDomainObjectBase
        {
            var domainType = context.Workflow.Logics.DomainType.GetByType(typeof(TDomain));

            var filterModel = new AvailableTaskInstanceMainFilterModel { DomainObjectId = workflowParameter.Id, SourceType = domainType };

            var requestItems = context.Workflow.Logics.TaskInstance.GetAvailableGroups(filterModel)
                .SelectMany(x => x.Items);

            return this.ToGroupExecuteCommandRequests(requestItems)
                .SelectMany(x => x.Split())
                .Single(z => z.Command.Name == commandName);
        }

        private IEnumerable<GroupExecuteCommandRequest> ToGroupExecuteCommandRequests(IEnumerable<AvailableTaskInstanceGroup> groups)
            => from taskInstanceGroup in groups
                   from taskInstanceGroupItem in taskInstanceGroup.Items
                   from command in taskInstanceGroupItem.Commands
                   from instance in taskInstanceGroupItem.TaskInstances
                   group instance by command
                   into grp
                   select this.CreateGroupExecuteCommandRequest(grp.Key, grp);

        private GroupExecuteCommandRequest CreateGroupExecuteCommandRequest(Command command, IEnumerable<TaskInstance> instances)
        {
            var parameters = CreateCommandParameters(command);

            return new GroupExecuteCommandRequest
            {
                Command = command,
                TaskInstances = instances.Distinct().ToList(),
                Parameters = parameters
            };
        }

        private static List<ExecuteCommandRequestParameter> CreateCommandParameters(Command command)
            => command.Parameters
                .Where(p => !p.IsReadOnly)
                .Select(p => new ExecuteCommandRequestParameter { Definition = p, Value = string.Empty })
                .ToList();
    }
}
