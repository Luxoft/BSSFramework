using System.CodeDom;

using Framework.BLL.Domain.DTO;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View._Base;

public abstract class ViewCollectionMethodGenerator<TConfiguration>(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
    : ViewMethodGenerator<TConfiguration>(configuration, domainType, dtoType)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected sealed override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO
                                                                  .GetCodeTypeReference(this.DomainType, this.DTOType)
                                                                  .ToEnumerableReference();
}
