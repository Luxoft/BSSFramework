using System.Linq.Expressions;

using CommonFramework;

namespace Framework.Persistent;

public record DeepLevelInfo<TDomainObject>(Expression<Func<TDomainObject, int>> Path)
{
    public Action<TDomainObject, int> Setter { get; } = Path.ToSetLambdaExpression().Compile();
}
