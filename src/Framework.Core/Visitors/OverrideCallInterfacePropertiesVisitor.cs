using System.Linq.Expressions;

namespace Framework.Core.Visitors;

public class OverrideCallInterfacePropertiesVisitor : ExpressionVisitor
{
    private readonly ExpressionVisitor internalVisitor;


    public OverrideCallInterfacePropertiesVisitor(Type interfaceType)
    {
        if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

        this.internalVisitor = interfaceType.GetProperties()
                                             .Select(property => new OverrideCallInterfacePropertyVisitor(property))
                                             .ToComposite();
    }


    public override Expression? Visit(Expression? node) => this.internalVisitor.Visit(node);
}
