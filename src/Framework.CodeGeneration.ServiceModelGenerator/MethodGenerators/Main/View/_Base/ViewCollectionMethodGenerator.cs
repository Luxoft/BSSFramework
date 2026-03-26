using System.CodeDom;

using Framework.CodeDom;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

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
