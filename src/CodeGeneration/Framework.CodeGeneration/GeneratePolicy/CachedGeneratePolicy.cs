using Anch.Core.DictionaryCache;

using Framework.Core;

namespace Framework.CodeGeneration.GeneratePolicy;

public class CachedGeneratePolicy<TIdent> : IGeneratePolicy<TIdent>
{
    private readonly IGeneratePolicy<TIdent> baseGeneratePolicy;

    private readonly IDictionaryCache<Tuple<Type, TIdent>, bool> cache;



    public CachedGeneratePolicy(IGeneratePolicy<TIdent> baseGeneratePolicy)
    {
        if (baseGeneratePolicy == null) throw new ArgumentNullException(nameof(baseGeneratePolicy));

        this.baseGeneratePolicy = baseGeneratePolicy;

        this.cache = new DictionaryCache<Tuple<Type, TIdent>, bool>(t => t.Pipe(this.InternalUsed)).WithLock();
    }



    public bool Used(Type domainType, TIdent fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return this.cache.GetValue(domainType, fileType);
    }


    protected virtual bool InternalUsed(Type domainType, TIdent fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return this.baseGeneratePolicy.Used(domainType, fileType);
    }
}
