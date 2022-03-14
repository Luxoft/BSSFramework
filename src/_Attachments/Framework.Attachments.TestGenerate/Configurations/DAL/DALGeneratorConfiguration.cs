﻿using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate.DALGenerator;

namespace Framework.Attachments.TestGenerate
{
    public class DALGeneratorConfiguration : GeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public DALGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }


        public override DatabaseName DatabaseName => this.Environment.DatabaseName;
    }
}
