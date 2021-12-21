using System;
using System.Collections;
using System.Collections.Generic;

using Framework.DomainDriven.BLLCoreGenerator;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;
using Framework.Projection;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator
{
    public class QueryServiceMethodGeneratorCollection<TConfiguration> : GeneratorConfigurationContainer<TConfiguration>, IEnumerable<IServiceMethodGenerator>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        private readonly Type _domainType;

        private readonly ViewDTOType _dtoType;


        public QueryServiceMethodGeneratorCollection(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration)
        {
            if (!Enum.IsDefined(typeof(ViewDTOType), dtoType)) throw new ArgumentOutOfRangeException(nameof(dtoType));

            this._domainType = domainType ?? throw new ArgumentNullException(nameof(domainType));
            this._dtoType = dtoType;
        }


        public IEnumerator<IServiceMethodGenerator> GetEnumerator()
        {
            yield return new GetODataListByQueryStringMethodGenerator<TConfiguration>(this.Configuration, this._domainType, this._dtoType);

            if (this.Configuration.Environment.ServerDTO.TypesWithSecondarySecurityOperations.ContainsKey(this._domainType))
            {
                yield return new GetODataListByQueryStringWithOperationMethodGenerator<TConfiguration>(this.Configuration, this._domainType, this._dtoType);

                if (this._domainType.GetProjectionSourceTypeOrSelf().IsHierarchical())
                {
                    yield return new GetODataTreeByQueryStringWithOperationMethodGenerator<TConfiguration>(this.Configuration, this._domainType, this._dtoType);
                }
            }


            foreach (var filterModelType in this.GetODataFilterTypes())
            {
                filterModelType.CheckDirectMode(DirectMode.Out, true);

                yield return new GetODataListByQueryStringWithFilterMethodGenerator<TConfiguration>(this.Configuration, this._domainType, this._dtoType, filterModelType);
            }

            foreach (var filterModelType in this.GetODataTreeFilterTypes())
            {
                filterModelType.CheckDirectMode(DirectMode.Out, true);

                yield return new GetODataTreeByQueryStringWithFilterMethodGenerator<TConfiguration>(this.Configuration, this._domainType, this._dtoType, filterModelType);
            }
        }

        private IEnumerable<Type> GetODataFilterTypes()
        {
            foreach (var filterModelType in this.Configuration.Environment.GetModelTypes(this._domainType, this.Configuration.Environment.BLLCore.ODataFilterModelType))
            {
                yield return filterModelType;
            }

            foreach (var filterModelType in this.Configuration.Environment.GetModelTypes(this._domainType, this.Configuration.Environment.BLLCore.ODataContextFilterModelType))
            {
                yield return filterModelType;
            }

            if (this._domainType.IsProjection())
            {
                foreach (var filterModelType in this._domainType.GetProjectionFilters(ProjectionFilterTargets.OData))
                {
                    yield return filterModelType;
                }
            }
        }


        private IEnumerable<Type> GetODataTreeFilterTypes()
        {
            if (this._domainType.IsProjection(sourceType => sourceType.IsHierarchical()))
            {
                foreach (var filterModelType in this._domainType.GetProjectionFilters(ProjectionFilterTargets.ODataTree))
                {
                    yield return filterModelType;
                }
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
