using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.Configuration._Container;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.GeneratePolicy;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.Core;

namespace Framework.CodeGeneration.ServiceModelGenerator.DTOGeneratePolicy;

public class DTOServiceGeneratePolicy<TConfiguration> : GeneratorConfigurationContainer<TConfiguration>, IGeneratePolicy<RoleFileType>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly HashSet<Tuple<Type, RoleFileType>> cache;


    public DTOServiceGeneratePolicy(TConfiguration configuration)
            : base(configuration)
    {
        var request = from domainType in this.Configuration.DomainTypes

                      from methodGenerator in this.Configuration.GetActualMethodGenerators(domainType)

                      let contractMethod = methodGenerator.GetContractMethod()

                      from baseTypeReference in contractMethod.ReturnType.MaybeYield().Concat(contractMethod.Parameters.ToArrayExceptNull(v => v).Select(v => v.Type))

                      from typeReference in baseTypeReference.GetReferenced()

                      let typeRefDomainType = typeReference.UserData["DomainType"] as Type

                      let typeFileType = typeReference.UserData["FileType"] as RoleFileType

                      where typeRefDomainType != null && typeFileType != null

                      select Tuple.Create(typeRefDomainType, typeFileType);

        this.cache = request.ToHashSet();
    }


    public bool Used(Type domainType, RoleFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return this.cache.Contains(new Tuple<Type, RoleFileType>(domainType, fileType));
    }
}
