using System;
using System.IO;

using Framework.Core;
using Framework.Attachments.TestGenerate;
using Framework.DomainDriven.Generation;

namespace Framework.Attachments.WebApiGenerate
{
    public partial class WebApiGenerators : GeneratorsBase
    {
        private readonly WebApiGenerationEnvironment Environment = new WebApiGenerationEnvironment();

        private string WebApiNetCorePath => Path.Combine(this.GeneratePath, "Framework.Attachments.WebApi", @"Controllers/_Generated");

        private static string FrameworkPath { get; } = System.Environment.CurrentDirectory.Replace(@"\", @"/").TakeWhileNot(@"/src", StringComparison.InvariantCultureIgnoreCase) + @"/src";

        private ICheckOutService CheckOutService { get; } = Framework.DomainDriven.Generation.CheckOutService.Trace;

        protected override string GeneratePath => FrameworkPath + @"/_Attachments";
    }
}
