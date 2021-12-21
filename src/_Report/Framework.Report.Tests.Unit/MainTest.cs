using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Report.Tests
{
    [TestClass]
    public class MainTest
    {
        [TestMethod]
        public void TestText001()
        {
            var sampleTemplate = "#{InternalObj.TestProperty} and #{#TestVariable.ToString()}";

            var testObj = new SampleObject1
            {
                InternalObj = new SampleObject2
                {
                    TestProperty = "hello"
                }
            };

            var testVariables = new Dictionary<string, object>
            {
                {"TestVariable", 123.4567}
            };

            var res = Spring.Text.TextTemplateEvaluator.Default.Evaluate(sampleTemplate, testObj, testVariables);

            Assert.AreEqual(res, $"hello and {123.4567}");
        }

        [TestMethod]
        public void RazorTest()
        {
            var template = "Test @Model.TestProperty.";

            var obj = new SampleObject2()
            {
                TestProperty = "TestProperty"
            };

            var result = Razor.Text.TextTemplateEvaluator.Default.Evaluate(template, obj);

            Assert.AreEqual(result, "Test TestProperty.");
        }
    }

    public class SampleObject1
    {
        public SampleObject2 InternalObj { get; set; }
    }

    public class SampleObject2
    {
        public string TestProperty { get; set; }
    }
}
