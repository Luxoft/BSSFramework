using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class DTOServiceGeneratePolicy<TConfiguration> : GeneratorConfigurationContainer<TConfiguration>, IGeneratePolicy<RoleFileType>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly HashSet<Tuple<Type, RoleFileType>> _cache;


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

        this._cache = request.ToHashSet();
    }


    public bool Used(Type domainType, RoleFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return this._cache.Contains(new Tuple<Type, RoleFileType>(domainType, fileType));
    }
}
