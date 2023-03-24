using System;
using System.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DTOGenerator;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public abstract class BaseSaveMethodGenerator<TConfiguration> : MethodGenerator<TConfiguration, BLLSaveRoleAttribute>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected readonly CodeTypeReference IdentTypeRef;

    protected readonly CodeTypeReference SourceIdentTypeRef;

    protected readonly CodeTypeReference StrictTypeRef;


    protected BaseSaveMethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
        this.IdentTypeRef = this.Configuration
                                .Environment.ServerDTO
                                .GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO);

        this.SourceIdentTypeRef = this.Configuration
                                      .Environment.ServerDTO
                                      .GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO);

        this.StrictTypeRef = this.Configuration
                                 .Environment.ServerDTO
                                 .GetCodeTypeReference(this.DomainType, DTOType.StrictDTO);
    }


    protected sealed override bool IsEdit { get; } = true;

    //protected override BLLSaveRoleAttribute GetDefaultAttribute()
    //{
    //    return new BLLSaveRoleAttribute { AllowCreate = this.DomainType.HasDefaultConstructor() };
    //}
}
