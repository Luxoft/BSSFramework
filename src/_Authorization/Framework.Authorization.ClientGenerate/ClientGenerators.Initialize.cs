using System;

using Framework.Authorization.TestGenerate;

namespace Framework.Authorization.ClientGenerate
{
    public partial class ClientGenerators : GeneratorsBase
    {
        private readonly ClientGenerationEnvironment Environment = new ClientGenerationEnvironment();

        protected override string GeneratePath => this.FrameworkPath + @"\src.gui";
    }
}
