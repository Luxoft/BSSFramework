using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Framework.Workflow.BLL;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Workflow.BLL.Tests.Unit.FiltersTests
{
    [TestClass]
    public class WorkflowInstanceRootFilterModelTest : BaseUnitTest
    {
        protected IWorkflowBLLContext _context;
        private const string _testWfName = "TestWfInstance";
        private const string _testWfParamValue = "TestParamValue";

        [TestInitialize]
        public void Initialize()
        {
            this._context = this.GetContext();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this._context = null;
        }

        protected bool TestWorkflowToFilterExpressionInvoke(WorkflowInstanceRootFilterModel filterModel)
        {
            var workflow = this._context.Logics.Workflow.Create(new WorkflowCreateModel());

            var filterResult = filterModel.ToFilterExpression();

            var result = filterResult != null;
            if (result)
            {
                var compiled = filterResult.Compile();
                var wfInstance = new WorkflowInstance(workflow, null);
                //wfInstance.IsFinal = filterModel.IsFinal;
                wfInstance.Name = _testWfName;
                var param = new WorkflowInstanceParameter(wfInstance);
                param.Value = _testWfParamValue;

                result = compiled.Invoke(wfInstance);
            }

            return result;
        }


        [TestMethod]
        public void TestWorkflowToFilterExpression_Null()
        {
            var filterModel = new WorkflowInstanceRootFilterModel
                {
                    IsFinal = null,
                };

            Assert.IsTrue(this.TestWorkflowToFilterExpressionInvoke(filterModel));
        }

        [TestMethod]
        public void TestWorkflowToFilterExpression_IsFinal_True()
        {
            var filterModel = new WorkflowInstanceRootFilterModel
            {
                IsFinal = true,
            };

            Assert.IsFalse(this.TestWorkflowToFilterExpressionInvoke(filterModel));
        }

        [TestMethod]
        public void TestWorkflowToFilterExpression_IsFinal_False()
        {
            var filterModel = new WorkflowInstanceRootFilterModel
            {
                IsFinal = false,
            };

            Assert.IsTrue(this.TestWorkflowToFilterExpressionInvoke(filterModel));
        }

        //[TestMethod]
        //public void TestWorkflowToFilterExpression_Name_Equal()
        //{
        //    var filterModel = new WorkflowInstanceRootFilterModel
        //    {
        //        IsFinal = false,
        //        Name = _testWfName,
        //        DomainObjectParameterValue = null
        //    };

        //    Assert.IsTrue(TestWorkflowToFilterExpressionInvoke(filterModel));
        //}

        //[TestMethod]
        //public void TestWorkflowToFilterExpression_Name_NotEqual()
        //{
        //    var filterModel = new WorkflowInstanceRootFilterModel
        //    {
        //        IsFinal = false,
        //        Name = "SomeOtherName",
        //        DomainObjectParameterValue = null
        //    };

        //    Assert.IsFalse(TestWorkflowToFilterExpressionInvoke(filterModel));
        //}

        //[TestMethod]
        //public void TestWorkflowToFilterExpression_DomainObjectParameterValue_Equal()
        //{
        //    var filterModel = new WorkflowInstanceRootFilterModel
        //    {
        //        IsFinal = false,
        //        Name = null,
        //        DomainObjectParameterValue = _testWfParamValue
        //    };

        //    Assert.IsTrue(TestWorkflowToFilterExpressionInvoke(filterModel));
        //}

        //[TestMethod]
        //public void TestWorkflowToFilterExpression_DomainObjectParameterValue_NotEqual()
        //{
        //    var filterModel = new WorkflowInstanceRootFilterModel
        //    {
        //        IsFinal = false,
        //        Name = null,
        //        DomainObjectParameterValue = "Something"
        //    };

        //    Assert.IsFalse(TestWorkflowToFilterExpressionInvoke(filterModel));
        //}

    }
}
