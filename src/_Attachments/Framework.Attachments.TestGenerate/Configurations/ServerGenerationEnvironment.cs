using System.Collections.Generic;
using System.Reflection;

using Framework.Attachments.Domain;

namespace Framework.Attachments.TestGenerate
{
    public class ServerGenerationEnvironment : Framework.Configuration.TestGenerate.ServerGenerationEnvironment
    {
        public ServerGenerationEnvironment()
        {
        }

        protected override IEnumerable<Assembly> GetDomainObjectAssemblies()
        {
            yield return typeof(AttachmentContainer).Assembly;
        }

        public new static readonly ServerGenerationEnvironment Default = new ServerGenerationEnvironment();
    }
}
