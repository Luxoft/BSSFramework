using System;
using System.Collections.Generic;
using System.Reflection;

using Framework.Attachments.Domain;
using Framework.Configuration.Domain;
using Framework.Configuration.TestGenerate;
using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.Attachments.TestGenerate
{
    public class ServerGenerationEnvironment : Framework.Configuration.TestGenerate.ServerGenerationEnvironment
    {
        //private AttachmentServerDTOGeneratorConfiguration serverDTO;

        protected ServerGenerationEnvironment()
        {
        }

        //public override ServerDTOGeneratorConfiguration ServerDTO => this.serverDTO ??= new AttachmentServerDTOGeneratorConfiguration(this);

        protected override IEnumerable<Assembly> GetDomainObjectAssemblies()
        {
            yield return typeof(AttachmentContainer).Assembly;
            yield return new ExtAssembly();
        }

        public override string TargetSystemName => "Attachments";

        public new static readonly ServerGenerationEnvironment Default = new ServerGenerationEnvironment();

        private class ExtAssembly : Assembly
        {
            public override Type[] GetTypes()
            {
                return new[] { typeof(DomainType) };
            }
        }
    }

    //public class AttachmentServerDTOGeneratorConfiguration : ServerDTOGeneratorConfiguration
    //{
    //    public AttachmentServerDTOGeneratorConfiguration(Configuration.TestGenerate.ServerGenerationEnvironment environment) : base(environment)
    //    {
    //    }

    //    protected override IGeneratePolicy<RoleFileType> CreateGeneratePolicy() => base.CreateGeneratePolicy().Except(typeof(DomainType));
    //}
}
