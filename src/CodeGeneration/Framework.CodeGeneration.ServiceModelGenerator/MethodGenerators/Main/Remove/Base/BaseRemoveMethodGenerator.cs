using System.CodeDom;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators._Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.Remove.Base;

public abstract class BaseRemoveMethodGenerator<TConfiguration> : MethodGenerator<TConfiguration, BLLRemoveRoleAttribute>
        where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    protected readonly CodeTypeReference IdentTypeRef;


    protected BaseRemoveMethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType) =>
        this.IdentTypeRef = this.Configuration
                                .Environment.ServerDTO
                                .GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO);

    protected sealed override CodeTypeReference ReturnType { get; } = typeof(void).ToTypeReference();

    protected sealed override bool IsEdit { get; } = true;
}
