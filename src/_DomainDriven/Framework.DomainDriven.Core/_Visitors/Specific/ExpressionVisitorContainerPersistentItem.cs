using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven;

public class ExpressionVisitorContainerPersistentItem : IExpressionVisitorContainerItem
{
    public IEnumerable<ExpressionVisitor> GetVisitors()
    {
        return new[]
               {
                       typeof(IVisualIdentityObject),
                       typeof(ICodeObject<>),
                       typeof(IDomainType),
                       typeof(IIdentityObject<>),
                       typeof(IParentSource<>),
                       typeof(IChildrenSource<>),
                       typeof(IEmployee)
               }.ToReadOnlyCollection(type => new OverrideCallInterfacePropertiesVisitor(type));
    }
}
