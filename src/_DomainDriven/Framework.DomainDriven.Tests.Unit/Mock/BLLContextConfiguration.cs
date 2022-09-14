using System;
using System.Collections.Generic;
using System.Reflection;
using Framework.DomainDriven.BLL;
using Framework.Persistent;

using NSubstitute;

namespace Framework.DomainDriven.UnitTest.Mock
{
    public abstract class BLLContextConfiguration<TBLLContext, TPersistentDomainObjectBase> : BLLContextConfiguration<TBLLContext, TPersistentDomainObjectBase, Guid> where TBLLContext : class, IBLLBaseContextBase<TPersistentDomainObjectBase, Guid> where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    {
        protected BLLContextConfiguration(IEnumerable<Assembly> domainAssemblies) : base(domainAssemblies)
        {

        }
    }

    public abstract class BLLContextConfiguration<TBLLContext, TPersistentDomainObjectBase, TIdent>

        where TBLLContext : class,  IBLLBaseContextBase<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        private readonly Lazy<TBLLContext> _bllContextLazy;

        protected BLLContextConfiguration(IEnumerable<Assembly> domainAssemblies)
        {
            this._bllContextLazy = new Lazy<TBLLContext>(this.CreateContext);
        }

        public TBLLContext Context
        {
            get { return this._bllContextLazy.Value; }
        }

        private TBLLContext CreateContext()
        {
            var result = Substitute.For<TBLLContext>();

            this.Initialize(result);

            return result;

        }

        protected abstract void Initialize<T>(T result) where T : class, TBLLContext;
    }
}
