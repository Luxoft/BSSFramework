using System;

using Framework.Workflow.TestGenerate;

namespace Framework.Workflow.ClientGenerate
{
    public partial class ClientGenerators : GeneratorsBase
    {
        private readonly ClientGenerationEnvironment Environment = new ClientGenerationEnvironment();

        protected override string GeneratePath => this.FrameworkPath + @"\src.gui";
    }
}
