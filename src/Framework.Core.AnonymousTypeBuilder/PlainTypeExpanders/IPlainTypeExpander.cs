using System;

namespace Framework.Core
{
    public interface IPlainTypeExpander
    {
        IExpressionConverter GetExpressionConverter(Type type);
    }
}