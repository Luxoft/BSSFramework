using System;
using System.CodeDom;
using System.ComponentModel;

using Framework.CodeDom;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class DefaultStructFileFactory<TConfiguration> : DTOFileFactory<TConfiguration, DTOFileType>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public DefaultStructFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }


        public override DTOFileType FileType { get; } = ClientFileType.Struct;


        protected override bool? InternalBaseTypeContainsPropertyChange { get; } = false;

        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                IsStruct = true,
                IsPartial = true,
                Attributes = MemberAttributes.Public
            };
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            foreach (var baseType in base.GetBaseTypes())
            {
                yield return baseType;
            }

            yield return typeof(INotifyPropertyChanging).ToTypeReference();
            yield return typeof(INotifyPropertyChanged).ToTypeReference();
        }
    }
}