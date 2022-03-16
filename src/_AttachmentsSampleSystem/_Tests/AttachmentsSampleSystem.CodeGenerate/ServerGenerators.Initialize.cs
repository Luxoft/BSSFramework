using System;
using System.IO;

using Framework.Core;
using Framework.DomainDriven.Generation;

namespace AttachmentsSampleSystem.CodeGenerate
{
    public partial class ServerGenerators
    {
        public ServerGenerators()
        {
            this.environment = new ServerGenerationEnvironment();
        }

        private readonly ServerGenerationEnvironment environment;

        private readonly string webApiNetCorePath = Path.Combine(TargetSystemPath, "AttachmentsSampleSystem.WebApiCore", @"Controllers/_Generated");

        private static string FrameworkPath { get; } = Environment.CurrentDirectory.Replace(@"\",@"/").TakeWhileNot(@"/src", StringComparison.InvariantCultureIgnoreCase) + @"/src";

        private static string TargetSystemPath => FrameworkPath + @"/_AttachmentsSampleSystem";

        private ICheckOutService CheckOutService { get; } = Framework.DomainDriven.Generation.CheckOutService.Trace;
    }
}
