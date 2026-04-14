using System.Collections;

using Framework.BLL.Domain.DirectMode;
using Framework.BLL.Domain.DTO;
using Framework.CodeGeneration.BLLCoreGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Query.OData;
using Framework.FileGeneration.Configuration;
using Framework.Projection;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.Query;

public class QueryServiceMethodGeneratorCollection<TConfiguration> : GeneratorConfigurationContainer<TConfiguration>, IEnumerable<IServiceMethodGenerator>
        where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    private readonly Type domainType;

    private readonly ViewDTOType dtoType;


    public QueryServiceMethodGeneratorCollection(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration)
    {
        if (!Enum.IsDefined(typeof(ViewDTOType), dtoType)) throw new ArgumentOutOfRangeException(nameof(dtoType));

        this.domainType = domainType ?? throw new ArgumentNullException(nameof(domainType));
        this.dtoType = dtoType;
    }


    public IEnumerator<IServiceMethodGenerator> GetEnumerator()
    {
        yield return new GetODataListByQueryStringMethodGenerator<TConfiguration>(this.Configuration, this.domainType, this.dtoType);

        yield return new GetODataListByQueryStringWithOperationMethodGenerator<TConfiguration>(this.Configuration, this.domainType, this.dtoType);

        if (this.Configuration.Environment.IsHierarchical(this.domainType.GetProjectionSourceTypeOrSelf()))
        {
            yield return new GetODataTreeByQueryStringWithOperationMethodGenerator<TConfiguration>(this.Configuration, this.domainType, this.dtoType);
        }

        foreach (var filterModelType in this.GetODataFilterTypes())
        {
            filterModelType.CheckDirectMode(DirectMode.Out, true);

            yield return new GetODataListByQueryStringWithFilterMethodGenerator<TConfiguration>(this.Configuration, this.domainType, this.dtoType, filterModelType);
        }

        foreach (var filterModelType in this.GetODataTreeFilterTypes())
        {
            filterModelType.CheckDirectMode(DirectMode.Out, true);

            yield return new GetODataTreeByQueryStringWithFilterMethodGenerator<TConfiguration>(this.Configuration, this.domainType, this.dtoType, filterModelType);
        }
    }

    private IEnumerable<Type> GetODataFilterTypes()
    {
        foreach (var filterModelType in this.Configuration.Environment.GetModelTypes(this.domainType, this.Configuration.Environment.BLLCore.ODataFilterModelType))
        {
            yield return filterModelType;
        }

        foreach (var filterModelType in this.Configuration.Environment.GetModelTypes(this.domainType, this.Configuration.Environment.BLLCore.ODataContextFilterModelType))
        {
            yield return filterModelType;
        }

        if (this.domainType.IsProjection())
        {
            foreach (var filterModelType in this.domainType.GetProjectionFilters(ProjectionFilterTargets.OData))
            {
                yield return filterModelType;
            }
        }
    }


    private IEnumerable<Type> GetODataTreeFilterTypes()
    {
        if (this.domainType.IsProjection(this.Configuration.Environment.IsHierarchical))
        {
            foreach (var filterModelType in this.domainType.GetProjectionFilters(ProjectionFilterTargets.ODataTree))
            {
                yield return filterModelType;
            }
        }
    }


    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
