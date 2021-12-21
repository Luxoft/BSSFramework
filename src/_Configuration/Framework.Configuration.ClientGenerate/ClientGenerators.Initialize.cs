using System;

using Framework.Configuration.TestGenerate;

namespace Framework.Configuration.ClientGenerate
{
    public partial class ClientGenerators : GeneratorsBase
    {
        private readonly ClientGenerationEnvironment Environment = new ClientGenerationEnvironment();

        protected override string GeneratePath => this.FrameworkPath + @"\src.gui";
    }
}
