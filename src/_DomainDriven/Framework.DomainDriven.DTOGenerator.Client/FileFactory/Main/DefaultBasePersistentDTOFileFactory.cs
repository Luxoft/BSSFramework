using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class DefaultBasePersistentDTOFileFactory<TConfiguration> : MainDTOFileFactoryBase<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public DefaultBasePersistentDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.PersistentDomainObjectBaseType)
        {
        }


        public override MainDTOFileType FileType { get; } = DTOGenerator.FileType.BasePersistentDTO;


        public override CodeTypeReference BaseReference => this.Configuration.GetBaseAbstractReference();

        public override CodeTypeReference CurrentInterfaceReference => this.Configuration.GetBasePersistentInterfaceReference();


        protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }


            yield return new CodeMemberProperty
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = "IsNew",
                Type = new CodeTypeReference(typeof(bool)),
                GetStatements =
                {
                    new CodeValueEqualityOperatorExpression(
                        this.Configuration.Environment.GetIdentityType().ToTypeReference().ToDefaultValueExpression(),
                        this.Configuration.GetIdentityPropertyCodeExpression()).ToMethodReturnStatement()
                }
            };
        }

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

        protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
        {
            foreach (var attr in base.GetCustomAttributes())
            {
                yield return attr;
            }

            var targetSystemAttr = this.Configuration.Environment.PersistentDomainObjectBaseType.GetCustomAttribute<TargetSystemAttribute>();

            if (targetSystemAttr != null)
            {
                var idArg = targetSystemAttr.Id.ToString().ToPrimitiveExpression().ToAttributeArgument();

                if (targetSystemAttr.Name == null)
                {
                    yield return typeof(TargetSystemAttribute).ToTypeReference().ToAttributeDeclaration(idArg);
                }
                else
                {
                    yield return typeof(TargetSystemAttribute).ToTypeReference().ToAttributeDeclaration(idArg, targetSystemAttr.Name.ToPrimitiveExpression().ToAttributeArgument());
                }
            }
        }
    }
}
