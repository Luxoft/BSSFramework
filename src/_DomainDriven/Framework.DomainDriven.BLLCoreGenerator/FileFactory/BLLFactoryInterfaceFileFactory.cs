using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL.Security;
using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class BLLFactoryInterfaceFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public BLLFactoryInterfaceFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {

    }


    public override FileType FileType => FileType.BLLFactoryInterface;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Name = this.Name,

                       Attributes = MemberAttributes.Public,
                       IsPartial = true,
                       IsInterface = true
               };
    }

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        var bllInterfaceTypeRef = this.Configuration.GetCodeTypeReference(this.DomainType, FileType.BLLInterface);

        //yield return typeof(IFactory<>).ToTypeReference(bllInterfaceTypeRef);

        Func<CodeTypeReference, CodeTypeReference> toSecurityBLLContainerTypeRef = securityObjectType => typeof(ISecurityBLLFactory<,>).ToTypeReference(bllInterfaceTypeRef, securityObjectType);

        var securityProviderTypeRef = typeof(ISecurityProvider<>).ToTypeReference(this.DomainType.ToTypeReference());

        yield return toSecurityBLLContainerTypeRef(securityProviderTypeRef);


        if (this.DomainType.IsSecurity())
        {
            if (this.Configuration.Environment.SecurityOperationCodeType.IsEnum)
            {
                yield return toSecurityBLLContainerTypeRef(this.Configuration.Environment.SecurityOperationCodeType.ToTypeReference());

                yield return toSecurityBLLContainerTypeRef(typeof(SecurityOperation<>).MakeGenericType(this.Configuration.Environment.SecurityOperationCodeType).ToTypeReference());
            }

            yield return toSecurityBLLContainerTypeRef(this.Configuration.GetBLLSecurityModeType(this.DomainType).ToTypeReference());
        }
    }
}
