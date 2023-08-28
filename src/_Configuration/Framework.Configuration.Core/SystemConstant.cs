using System.Linq.Expressions;

using Framework.Core;

namespace Framework.Configuration;

public class SystemConstant<T>
{
    public SystemConstant(Expression<Func<SystemConstant<T>>> codeExpr, T defaultValue, string description)
            : this(codeExpr.GetStaticMemberName(), defaultValue, description)
    {

    }

    public SystemConstant(string code, T defaultValue, string description)
    {
        if (code == null) throw new ArgumentNullException(nameof(code));

        this.Code = code;
        this.DefaultValue = defaultValue;
        this.Description = description;
    }


    public readonly string Code;

    public readonly T DefaultValue;

    public readonly string Description;
}
