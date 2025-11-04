using System.Linq.Expressions;

using CommonFramework;

using Framework.Core.Visitors;
using Framework.Persistent;

namespace Framework.DomainDriven._Visitors;

public class ExpressionVisitorContainerPersistentItem : IExpressionVisitorContainerItem
{
    public IEnumerable<ExpressionVisitor> GetVisitors()
    {
        return new[]
               {
                       typeof(IVisualIdentityObject),
                       typeof(ICodeObject<>),
                       typeof(IIdentityObject<>)
               }.ToReadOnlyCollection(type => new OverrideCallInterfacePropertiesVisitor(type));
    }
}
