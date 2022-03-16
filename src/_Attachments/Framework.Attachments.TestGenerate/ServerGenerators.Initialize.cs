using System;
using System.Linq;

namespace Framework.Attachments.TestGenerate
{
    public partial class ServerGenerators : GeneratorsBase
    {
        protected readonly ServerGenerationEnvironment Environment;

        private readonly bool genParallel = true;

        public ServerGenerators()
            : this(ServerGenerationEnvironment.Default)
        {
            this.Environment.ProjectionEnvironments.SelectMany(pe => pe.Assembly.GetTypes()).ToList();
        }

        public ServerGenerators(ServerGenerationEnvironment environment)
        {
            this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        protected override string GeneratePath => this.FrameworkPath + @"/src/_Attachments";
    }
}
