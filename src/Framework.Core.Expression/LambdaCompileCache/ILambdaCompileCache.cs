using System.Linq.Expressions;

namespace Framework.Core
{
    public interface ILambdaCompileCache
    {
        TDelegate GetFunc<TDelegate>(Expression<TDelegate> lambdaExpression);
    }
}