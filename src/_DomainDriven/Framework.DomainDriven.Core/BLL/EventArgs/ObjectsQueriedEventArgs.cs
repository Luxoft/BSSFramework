using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Framework.Core;

namespace Framework.DomainDriven.BLL
{
    public class ObjectsQueriedEventArgs<T>: EventArgsWithResult<List<T>>
    {
        private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache();

        private readonly Expression<Func<T, bool>> query;
        private Func<T, bool> compiledQuery;

        public ObjectsQueriedEventArgs(List<T> content, Expression<Func<T, bool>> query)
            : base(content)
        {
            this.query = query;
        }

        public Expression<Func<T, bool>> Query
        {
            get { return this.query; }
        }

        public Func<T, bool> CompiledQuery
        {
            get { return this.compiledQuery ?? (this.compiledQuery = this.query.Compile(LambdaCompileCache)); }
        }
    }
}
