using CommonFramework.DictionaryCache;

using Framework.Core;

namespace Framework.DomainDriven.Generation.Domain;

public class CachedGeneratePolicy<TIdent> : IGeneratePolicy<TIdent>
{
    private readonly IGeneratePolicy<TIdent> _baseGeneratePolicy;

    private readonly IDictionaryCache<Tuple<Type, TIdent>, bool> _cache;



    public CachedGeneratePolicy(IGeneratePolicy<TIdent> baseGeneratePolicy)
    {
        if (baseGeneratePolicy == null) throw new ArgumentNullException(nameof(baseGeneratePolicy));

        this._baseGeneratePolicy = baseGeneratePolicy;

        this._cache = new DictionaryCache<Tuple<Type, TIdent>, bool>(t => t.Pipe(this.InternalUsed)).WithLock();
    }



    public bool Used(Type domainType, TIdent fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return this._cache.GetValue(domainType, fileType);
    }


    protected virtual bool InternalUsed(Type domainType, TIdent fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return this._baseGeneratePolicy.Used(domainType, fileType);
    }
}
