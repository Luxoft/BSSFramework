﻿using System.Linq.Expressions;

using Framework.Core;
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
                       typeof(IDomainType),
                       typeof(IIdentityObject<>),
                       typeof(IParentSource<>),
                       typeof(IChildrenSource<>),
                       //typeof(IEmployee)
               }.ToReadOnlyCollection(type => new OverrideCallInterfacePropertiesVisitor(type));
    }
}
