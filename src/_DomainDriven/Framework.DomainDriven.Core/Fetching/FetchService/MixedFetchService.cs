using System;
using Framework.OData;

namespace Framework.DomainDriven
{
    public class MixedFetchService<TPersistentDomainObjectBase> : IFetchService<TPersistentDomainObjectBase, FetchBuildRule>
    {
        private readonly IFetchService<TPersistentDomainObjectBase, FetchBuildRule.DTOFetchBuildRule> _mainService;

        private readonly IFetchService<TPersistentDomainObjectBase, SelectOperation> _odataService;

        public MixedFetchService(IFetchService<TPersistentDomainObjectBase, FetchBuildRule.DTOFetchBuildRule> mainService, IFetchService<TPersistentDomainObjectBase, SelectOperation> odataService)
        {
            if (mainService == null) throw new ArgumentNullException(nameof(mainService));
            if (odataService == null) throw new ArgumentNullException(nameof(odataService));

            this._mainService = mainService;
            this._odataService = odataService;
        }

        public IFetchContainer<TDomainObject> GetContainer<TDomainObject>(FetchBuildRule rule)
            where TDomainObject : TPersistentDomainObjectBase
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));

            return rule.Match(this._mainService.GetContainer<TDomainObject>, this._odataService.GetContainer<TDomainObject>);
        }
    }
}