using System.CodeDom;

using Framework.BLL.Domain.DTO;
using Framework.CodeDom;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;

namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Main.View._Base;

public abstract class ViewCollectionMethodGenerator<TConfiguration> : ViewMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected ViewCollectionMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration, domainType, dtoType)
    {
    }


    protected sealed override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO
                                                                  .GetCodeTypeReference(this.DomainType, this.DTOType)
                                                                  .ToEnumerableReference();
}
