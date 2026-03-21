using System.Linq.Expressions;

using CommonFramework;

using Framework.Application._Visitors.ExpressionVisitorContainer;
using Framework.Application.Domain;
using Framework.Core.Visitors;

namespace Framework.Application._Visitors.Specific;

public class ExpressionVisitorContainerPersistentItem : IExpressionVisitorContainerItem
{
    public IEnumerable<ExpressionVisitor> GetVisitors()
    {
        return new[]
               {
                       typeof(IVisualIdentityObject),
                       typeof(IIdentityObject<>)
               }.ToReadOnlyCollection(type => new OverrideCallInterfacePropertiesVisitor(type));
    }
}
