using System.CodeDom;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;
using Framework.CodeGeneration.WebApiGenerator.MethodGenerators._Base;

namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Main.Save.Base;

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
