using System;

using Framework.Core;
using Framework.OData;

namespace Framework.DomainDriven
{
    public class FetchService<TPersistentDomainObjectBase, TBuildRule> : IFetchService<TPersistentDomainObjectBase, TBuildRule>
    {
        private readonly IFetchPathFactory<TBuildRule> _factory;

        public FetchService(IFetchPathFactory<TBuildRule> factory)
        {
            this._factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public IFetchContainer<TDomainObject> GetContainer<TDomainObject>(TBuildRule rule)
            where TDomainObject : TPersistentDomainObjectBase
        {
            return new FetchContainer<TDomainObject>(this._factory.Create(typeof(TDomainObject), rule));
        }
    }

    public static class FetchService<TPersistentDomainObjectBase>
    {
        public static readonly IFetchService<TPersistentDomainObjectBase, SelectOperation> OData = new FetchService<TPersistentDomainObjectBase, SelectOperation>(new ODataFetchPathFactory(typeof(TPersistentDomainObjectBase)));

        public static readonly IFetchService<TPersistentDomainObjectBase, FetchBuildRule.DTOFetchBuildRule> Main = new FetchService<TPersistentDomainObjectBase, FetchBuildRule.DTOFetchBuildRule>(new ExpandFetchPathFactory(typeof(TPersistentDomainObjectBase))).WithCompress().WithCache().WithLock();

        public static readonly IFetchService<TPersistentDomainObjectBase, FetchBuildRule> Mixed = Main.Add(OData);
    }
}
