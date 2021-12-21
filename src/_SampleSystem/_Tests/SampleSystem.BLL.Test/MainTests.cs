using System;

using Framework.ExpressionParsers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SampleSystem.NetCoreTests
{
    [TestClass]
    public class MainTests
    {
        [TestMethod]
        public void TestParser()
        {
            var parser = CSharpNativeExpressionParser.Compile;

            var del = parser.Parse<Func<int, int, int>>("(a, b) => a + b");

            return;
        }
    }
}
