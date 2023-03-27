using System.Linq.Expressions;

namespace Framework.Core;

public class OverrideCallInterfacePropertiesVisitor : ExpressionVisitor
{
    private readonly ExpressionVisitor _internalVisitor;


    public OverrideCallInterfacePropertiesVisitor(Type interfaceType)
    {
        if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

        this._internalVisitor = interfaceType.GetProperties()
                                             .Select(property => new OverrideCallInterfacePropertyVisitor(property))
                                             .ToComposite();
    }


    public override Expression Visit(Expression node)
    {
        return this._internalVisitor.Visit(node);
    }
}
