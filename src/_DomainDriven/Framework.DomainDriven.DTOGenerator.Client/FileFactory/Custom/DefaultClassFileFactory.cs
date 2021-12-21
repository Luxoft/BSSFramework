using System;
using System.CodeDom;
using System.ComponentModel;

using Framework.CodeDom;
using Framework.Reactive;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class DefaultClassFileFactory<TConfiguration> : DTOFileFactory<TConfiguration, DTOFileType>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public DefaultClassFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }


        public override DTOFileType FileType { get; } = ClientFileType.Class;

        public override CodeTypeReference BaseReference => this.HasBase

            ? this.Configuration.GetCodeTypeReference(this.DomainType.BaseType, ClientFileType.Class)

            : null;


        protected override bool? InternalBaseTypeContainsPropertyChange => this.HasBase;


        private bool HasBase => this.DomainType.BaseType != typeof(object);




        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            var classDeclaration = new CodeTypeDeclaration(this.Name)
            {
                IsClass = true,
                IsPartial = true,
                Attributes = MemberAttributes.Public
            };

            return classDeclaration;
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            foreach (var baseType in base.GetBaseTypes())
            {
                yield return baseType;
            }

            if (!this.HasBase)
            {
                yield return new CodeTypeReference(typeof(INotifyPropertyChanging)).ToTypeReference();
                yield return new CodeTypeReference(typeof(INotifyPropertyChanged)).ToTypeReference();
                yield return new CodeTypeReference(typeof(IBaseRaiseObject)).ToTypeReference();
            }

            yield return new CodeTypeReference(typeof(ICloneable<>)) {TypeArguments = { this.CurrentReference}};
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }


            yield return this.GenerateDefaultConstructor();

            yield return this.GenerateUnpersistentCloneConstructor();

            yield return CodeDomHelper.GenerateExplicitImplementationClone();

            yield return this.CurrentReference.GenerateCloneMethod(false);


            if (!this.HasBase)
            {
                yield return CodeDomHelper.GenerateExplicitImplementationBaseRaise();
            }
        }
    }
}