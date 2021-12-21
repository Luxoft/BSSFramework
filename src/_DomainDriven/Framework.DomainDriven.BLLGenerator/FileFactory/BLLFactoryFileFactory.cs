using System;
using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.BLLCoreGenerator;
using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLLGenerator
{
    public class BLLFactoryFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public BLLFactoryFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {

        }


        public override FileType FileType => FileType.BLLFactory;


        public CodeTypeReference BLLInterfaceRef => this.Configuration.Environment.BLLCore.GetCodeTypeReference(this.DomainType, BLLCoreGenerator.FileType.BLLInterface);

        public CodeTypeReference BLLRef => this.Configuration.GetCodeTypeReference(this.DomainType, FileType.BLL);

        public CodeTypeReference BLLFactoryInterfaceRef => this.Configuration.Environment.BLLCore.GetCodeTypeReference(this.DomainType, BLLCoreGenerator.FileType.BLLFactoryInterface);


        private Type SecurityCodeType => this.Configuration.Environment.SecurityOperationCodeType;

        private CodeTypeReference GetBaseReference()
        {
            if (this.DomainType.IsSecurity())
            {
                if (this.Configuration.Environment.SecurityOperationCodeType.IsEnum)
                {
                    return typeof(SecurityBLLFactory<,,,,>).ToTypeReference(this.Configuration.BLLContextTypeReference, this.BLLInterfaceRef, this.BLLRef, this.DomainType.ToTypeReference(), this.Configuration.Environment.SecurityOperationCodeType.ToTypeReference());
                }
                else
                {
                    return typeof(SecurityBLLFactoryBase<,,,>).ToTypeReference(this.Configuration.BLLContextTypeReference, this.BLLInterfaceRef, this.BLLRef, this.DomainType.ToTypeReference());
                }
            }
            else
            {
                return typeof(BLLFactoryBase<,,,>).ToTypeReference(this.Configuration.BLLContextTypeReference, this.BLLInterfaceRef, this.BLLRef, this.DomainType.ToTypeReference());
            }
        }

        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return this.Configuration.Environment.BLLCore.GetBLLContextContainerCodeTypeDeclaration(this.Name, false, this.GetBaseReference());
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            yield return this.BLLFactoryInterfaceRef;
        }
    }
}
