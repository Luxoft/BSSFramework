using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Framework.Graphviz;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Workflow.BLL.Tests.Unit.GraphvizTests
{
    [TestClass]
    public class ImageFormatWebModelTest
    {
        //[TestMethod]
        //public void FromString_Format_IsNull()
        //{
        //    var webModel = ImageFormatWebModel.FromString(null);

        //    Assert.IsNotNull(webModel);
        //    Assert.AreEqual(webModel.Extension, ImageFormatWebModel.DefaultImageFormat.Extension);
        //}

        [TestMethod]
        public void FromString_Format_Svg()
        {
            var webModel = GraphvizFormatMetadata.Svg;

            Assert.IsNotNull(webModel);
            Assert.AreEqual(webModel.Extension, "svg");
            Assert.AreEqual(webModel.ContentType, "image/svg+xml");
        }

        //[TestMethod]
        //public void FromString_Format_Incorrect()
        //{
        //    var webModel = ImageFormatWebModel.FromString("super_format");

        //    Assert.IsNotNull(webModel);
        //    Assert.AreEqual(webModel.Extension, ImageFormatWebModel.DefaultImageFormat.Extension);
        //}
    }
}
