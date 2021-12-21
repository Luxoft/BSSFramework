using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.Core;

namespace Framework.DomainDriven.BLL
{
    public abstract partial class BLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObjectBase, TDomainObject, TIdent, TOperation>
    {
        private class QueryProviderDecorator : IQueryProvider
        {
            private readonly BLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObjectBase, TDomainObject, TIdent, TOperation> _bllBase;

            private readonly IQueryProvider _source;

            public QueryProviderDecorator(BLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObjectBase, TDomainObject, TIdent, TOperation> bllBase, IQueryProvider source)
            {
                if (bllBase == null) throw new ArgumentNullException(nameof(bllBase));
                if (source == null) throw new ArgumentNullException(nameof(source));

                this._bllBase = bllBase;
                this._source = source;
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return new QueryableDecorator(this._bllBase, this._source.CreateQuery(this._bllBase.OverrideExpression(expression)));
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new QueryableDecorator<TElement>(this._bllBase, this._source.CreateQuery<TElement>(this._bllBase.OverrideExpression(expression)));
            }

            public object Execute(Expression expression)
            {
                return this._source.Execute(this._bllBase.OverrideExpression(expression));
            }

            public TResult Execute<TResult>(Expression expression)
            {
                var overrideExpression = this._bllBase.OverrideExpression(expression);
                return this._source.Execute<TResult>(overrideExpression);
            }
        }

        private class QueryableDecorator<TElement> : IOrderedQueryable<TElement>
        {
            private readonly BLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObjectBase, TDomainObject, TIdent, TOperation> _bllBase;

            private readonly IQueryable<TElement> _source;

            public QueryableDecorator(BLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObjectBase, TDomainObject, TIdent, TOperation> bllBase, IQueryable<TElement> source)
            {
                if (bllBase == null) throw new ArgumentNullException(nameof(bllBase));
                if (source == null) throw new ArgumentNullException(nameof(source));

                this._bllBase = bllBase;
                this._source = source;
            }


            public IEnumerator<TElement> GetEnumerator()
            {
                var newExpr = this.Expression.UpdateBase(OptimizeWhereAndConcatVisitor.Value);

                var newQ = this._source.Provider.CreateQuery<TElement>(newExpr);

                return newQ.GetEnumerator();
            }


            IEnumerator IEnumerable.GetEnumerator()
            {
                return this._source.GetEnumerator();
            }

            public Expression Expression
            {
                get { return this._source.Expression; }
            }

            public Type ElementType
            {
                get { return this._source.ElementType; }
            }

            public IQueryProvider Provider
            {
                get { return new QueryProviderDecorator(this._bllBase, this._source.Provider); }
            }

        }

        private class QueryableDecorator : IOrderedQueryable
        {
            private readonly BLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObjectBase, TDomainObject, TIdent, TOperation> _bllBase;
            private readonly IQueryable _source;

            public QueryableDecorator(BLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObjectBase, TDomainObject, TIdent, TOperation> bllBase, IQueryable source)
            {
                if (bllBase == null) throw new ArgumentNullException(nameof(bllBase));
                if (source == null) throw new ArgumentNullException(nameof(source));

                this._bllBase = bllBase;
                this._source = source;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this._source.GetEnumerator();
            }

            public Expression Expression
            {
                get { return this._source.Expression; }
            }

            public Type ElementType
            {
                get { return this._source.ElementType; }
            }

            public IQueryProvider Provider
            {
                get { return new QueryProviderDecorator(this._bllBase, this._source.Provider); }
            }
        }
    }
}