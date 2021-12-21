using System;
using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class DefaultFullDTOFileFactory<TConfiguration> : MainDTOFileFactory<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public DefaultFullDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }


        public override MainDTOFileType FileType { get; } = DTOGenerator.FileType.FullDTO;



        protected override System.Collections.Generic.IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            foreach (var baseType in base.GetBaseTypes())
            {
                yield return baseType;
            }

            var parentSourceType = typeof(IParentSource<>).MakeGenericType(this.DomainType);

            if (this.DomainType.IsInterfaceImplementation(parentSourceType) && this.DomainType.IsPropertyImplement(parentSourceType))
            {
                yield return typeof(IParentSource<>).ToTypeReference(this.Configuration.GetCodeTypeReference(this.DomainType, DTOGenerator.FileType.SimpleDTO));
            }
        }
    }
}