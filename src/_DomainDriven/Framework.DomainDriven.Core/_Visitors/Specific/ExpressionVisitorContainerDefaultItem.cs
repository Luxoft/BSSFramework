using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;

namespace Framework.DomainDriven;

public class ExpressionVisitorContainerDefaultItem<TBLLContext, TPersistentDomainObjectBase, TIdent> : IExpressionVisitorContainerItem
        where TBLLContext : class, IHierarchicalObjectExpanderFactoryContainer<TIdent>
{
    private readonly TBLLContext context;

    public ExpressionVisitorContainerDefaultItem(TBLLContext context)
    {
        this.context = context;
    }

    public IEnumerable<ExpressionVisitor> GetVisitors()
    {
        var idProperty = typeof(TPersistentDomainObjectBase).GetProperty("Id", () => new Exception("Id property not found"));


        yield return OptimizeBooleanLogicVisitor.Value;
        yield return OptimizeWhereAndConcatVisitor.Value;

        yield return RestoreQueryableCallsVisitor.Value;

        yield return OverrideInstanceContainsIdentMethodVisitor<TIdent>.HashSet;
        yield return OverrideInstanceContainsIdentMethodVisitor<TIdent>.CollectionInterface;

        // TODO gtsaplin: remove OverrideHashSetVisitor, NH4.0 and above support HashSet
        yield return OverrideHashSetVisitor<TIdent>.Value;

        yield return OverrideListContainsVisitor<TIdent>.GetOrCreate(idProperty);
        yield return OverrideEqualsDomainObjectVisitor<TIdent>.GetOrCreate(idProperty);
        yield return OverrideIdEqualsMethodVisitor<TIdent>.GetOrCreate(idProperty);
        yield return OverrideHasFlagVisitor.Value;
        yield return ExpandPathVisitor.Value;
        yield return EscapeUnderscoreVisitor.Value;

        yield return new OverrideExpandContainsVisitor<TBLLContext, TIdent>(this.context, idProperty);
    }
}
