using System.Linq.Expressions;

using CommonFramework;

namespace Framework.Persistent;

public record DeepLevelInfo<TDomainObject>(PropertyAccessors<TDomainObject, int> DeepLevel)
{
    public DeepLevelInfo(Expression<Func<TDomainObject, int>> deepLevelPath)
        : this(deepLevelPath.ToPropertyAccessors())
    {
    }
}
