using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;

using Framework.CodeDom;
using Framework.Reactive;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class DefaultBaseAbstractInterfaceDTOFileFactory<TConfiguration> : InterfaceDTOFileFactory<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public DefaultBaseAbstractInterfaceDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.DomainObjectBaseType)
        {
        }


        public override MainDTOInterfaceFileType FileType { get; } = ClientFileType.BaseAbstractInterfaceDTO;


        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                IsInterface = true,
                IsPartial = true,
                Attributes = MemberAttributes.Public | MemberAttributes.Abstract
            };
        }

        protected override IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            foreach (var baseType in base.GetBaseTypes())
            {
                yield return baseType;
            }

            if (this.Configuration.ContainsPropertyChange)
            {
                yield return typeof(INotifyPropertyChanging).ToTypeReference();
                yield return typeof(INotifyPropertyChanged).ToTypeReference();
                yield return typeof(IBaseRaiseObject).ToTypeReference();
            }
        }
    }
}