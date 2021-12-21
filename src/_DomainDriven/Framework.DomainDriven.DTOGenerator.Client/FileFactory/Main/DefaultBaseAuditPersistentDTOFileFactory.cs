using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

using Framework.CodeDom;
using Framework.DomainDriven.DTOGenerator.Client;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class DefaultBaseAuditPersistentDTOFileFactory<TConfiguration> : MainDTOFileFactoryBase<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public DefaultBaseAuditPersistentDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.AuditPersistentDomainObjectBaseType)
        {
        }


        public override MainDTOFileType FileType { get; } = DTOGenerator.FileType.BaseAuditPersistentDTO;

        public override CodeTypeReference CurrentInterfaceReference => this.Configuration.GetBaseAuditPersistentInterfaceReference();

        protected override IEnumerable<CodeConstructor> GetConstructors()
        {
            //foreach (var baseCtor in base.GetConstructors())
            //{
            //    yield return baseCtor;
            //}

            yield return this.GenerateDefaultConstructor();
            yield return this.GeneratePersistentCloneConstructor();
            yield return this.GeneratePersistentCloneConstructorWithCopyIdParameter();
        }
    }
}