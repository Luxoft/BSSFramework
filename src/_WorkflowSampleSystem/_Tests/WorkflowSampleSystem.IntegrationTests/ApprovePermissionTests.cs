using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using FluentAssertions;

using Framework.Core;
using Framework.Workflow.Generated.DTO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WorkflowSampleSystem.IntegrationTests.__Support.TestData;

namespace WorkflowSampleSystem.IntegrationTests.Workflow
{
    [TestClass]
    public class ApprovePermissionTests : TestBase
    {
        [TestMethod]
        public void PerformTransition_TransitionVersionShouldNotBeChanged()
        {
            // Arrange
            var wfFacade = this.GetWorkflowControllerEvaluator();

            var wf = new DataContractSerializer(typeof(WorkflowStrictDTO)).ReadFromFile<WorkflowStrictDTO>(@"NewApprovePermission.xml");

            var res = wfFacade.Evaluate(c => c.SaveWorkflow(wf));

            // Act

            // Assert

            return;
        }
    }
}
