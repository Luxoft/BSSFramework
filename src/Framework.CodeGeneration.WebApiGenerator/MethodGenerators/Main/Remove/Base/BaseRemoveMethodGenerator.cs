using System.CodeDom;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeDom;
using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;
using Framework.CodeGeneration.WebApiGenerator.MethodGenerators._Base;

namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Main.Remove.Base;

public abstract class BaseRemoveMethodGenerator<TConfiguration> : MethodGenerator<TConfiguration, BLLRemoveRoleAttribute>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected readonly CodeTypeReference IdentTypeRef;


    protected BaseRemoveMethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
        this.IdentTypeRef = this.Configuration
                                .Environment.ServerDTO
                                .GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO);
    }


    protected sealed override CodeTypeReference ReturnType { get; } = typeof(void).ToTypeReference();

    protected sealed override bool IsEdit { get; } = true;
}
