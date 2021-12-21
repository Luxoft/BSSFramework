using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class DefaultBaseAbstractDTOFileFactory<TConfiguration> : MainDTOFileFactoryBase<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public DefaultBaseAbstractDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.DomainObjectBaseType)
        {
            this.CurrentInterfaceReference = this.Configuration.GetBaseAbstractInterfaceReference();
        }


        public override MainDTOFileType FileType { get; } = DTOGenerator.FileType.BaseAbstractDTO;


        public override CodeTypeReference CurrentInterfaceReference { get; }

        protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            yield return CodeDomHelper.GenerateExplicitImplementationBaseRaise();
        }


        protected override IEnumerable<CodeConstructor> GetConstructors()
        {
            var ctor = this.GenerateUnpersistentCloneConstructor();

            ctor.ChainedConstructorArgs.Add(new CodeSnippetExpression(""));

            yield return ctor;
        }
    }
}