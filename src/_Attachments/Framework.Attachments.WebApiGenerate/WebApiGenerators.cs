using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Framework.CodeDom;
using Framework.DomainDriven.Generation;
using Framework.DomainDriven.ServiceModelGenerator;
using Framework.DomainDriven.WebApiGenerator.NetCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FileInfo = Framework.DomainDriven.Generation.FileInfo;

namespace Framework.Attachments.WebApiGenerate
{
    [TestClass]
    public partial class WebApiGenerators
    {
        [TestMethod]
        public void GenerateMainTest()
        {
            this.GenerateAttachmentsWebApiNetCoreTest().ToList();
        }


        public IEnumerable<FileInfo> GenerateAttachmentsWebApiNetCoreTest()
        {
            var configurators = new Framework.DomainDriven.ServiceModelGenerator.IGeneratorConfigurationBase<Framework.DomainDriven.ServiceModelGenerator.IGenerationEnvironmentBase>[]
                                {
                                        this.Environment.MainService,
                                };

            var attr = new CodeAttributeDeclaration(
                                                    typeof(RouteAttribute).ToTypeReference(),
                                                    new CodeAttributeArgument(new CodePrimitiveExpression("AttachmentsApi/v{version:apiVersion}/[controller]")));

            var generator = new WebApiNetCoreFileGenerator(this.Environment.ToWebApiNetCore(configurators, nameSpace: "Framework.Attachments.WebApi"), additionalControllerAttributes: new[] { attr }.ToList());

            return generator.DecorateProjectionsToRootControllerNetCore().WithAutoRequestMethods().Generate<CodeNamespace>(Path.Combine(this.WebApiNetCorePath, "Attachments"));
        }
    }
}
