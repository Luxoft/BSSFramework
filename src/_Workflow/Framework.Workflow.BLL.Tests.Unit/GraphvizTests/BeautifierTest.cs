using Framework.Graphviz.Dot;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Workflow.BLL.Tests.Unit.GraphvizTests
{
    [TestClass]
    public class BeautifierTest
    {
        [TestMethod]
        public void BeautifierTest_Main()
        {
            var dot = "Node [shape = box] edge [color= red] Init->Draft[label='Sambuk 15_05_2013' fontsize = 8 shape = box]";
            var dotBeautified = DotBeautifier.OptmizeDot(dot);

            Assert.AreNotEqual(dot, dotBeautified);
        }
    }
}
