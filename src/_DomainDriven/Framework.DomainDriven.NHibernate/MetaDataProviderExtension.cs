using Framework.Core;

using NHibernate.Cfg;
using NHibernate.Envers.Configuration.Store;

namespace Framework.DomainDriven.NHibernate;

public static class MetaDataProviderExtension
{
    public static IMetaDataProvider Combine(this IEnumerable<IMetaDataProvider> providers)
    {
        return new MetaDataProviderComposite(providers);
    }

    private class MetaDataProviderComposite : IMetaDataProvider
    {
        private IList<IMetaDataProvider> _providers;

        public MetaDataProviderComposite(IEnumerable<IMetaDataProvider> providers)
        {
            if (providers == null)
            {
                throw new ArgumentNullException(nameof(providers));
            }

            this._providers = providers.ToList();
        }

        public IDictionary<Type, IEntityMeta> CreateMetaData(Configuration nhibConfiguration)
        {
            var selected = this._providers.Select(z => z.CreateMetaData(nhibConfiguration));
            return selected.Aggregate(
                                      (Dictionary<Type, IEntityMeta>)(new Dictionary<Type, IEntityMeta>()),
                                      (left, right) =>
                                      {
                                          right.Foreach(pair =>
                                                        {
                                                            if (!left.ContainsKey(pair.Key))
                                                            {
                                                                left.Add(pair.Key, pair.Value);
                                                            }
                                                        });
                                          return left;
                                      });
        }
    }
}
