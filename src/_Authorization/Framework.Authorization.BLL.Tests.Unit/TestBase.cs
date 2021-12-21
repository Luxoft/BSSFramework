using System;
using Framework.Authorization.BLL.Tests.Unit.Support;
using Framework.Authorization.Domain;
using Framework.DomainDriven.UnitTest.Mock;
using Framework.Persistent;

using NUnit.Framework;

namespace Framework.Authorization.BLL.Tests.Unit
{
    public abstract class TestBase
    {
        protected AuthorizationTestConfiguration Configuration;

        [SetUp]
        public virtual void BaseInitialize()
        {
            this.Configuration = new AuthorizationTestConfiguration();
        }

        [TearDown]
        public virtual void BaseCleanup()
        {
            this.Configuration = null;
        }

        public MockDalFactoryProvider<PersistentDomainObjectBase, Guid> MockDalFactory
        {
            get
            {
                return this.Configuration.MockDalFactory;
            }
        }
        public void RegisterDomainObject<T>(params T[] values) where T : IIdentityObject<Guid>
        {
            this.Configuration.MockDalFactory.Register(values);
        }

        public IAuthorizationBLLContext Context
        {
            get
            {
                return this.Configuration.Context;
            }
        }
    }
}