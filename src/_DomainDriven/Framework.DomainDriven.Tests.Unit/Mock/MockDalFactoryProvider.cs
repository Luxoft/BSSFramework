using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using NSubstitute;
using NSubstitute.Core;

namespace Framework.DomainDriven.UnitTest.Mock
{
    public class MockDalFactoryProvider<TPersistentDomainObjectBase, TIdent> where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        private readonly Dictionary<Type, IMockDAL> _mockDals;

        public MockDalFactoryProvider(IEnumerable<Assembly> domainAssemblies)
        {
            this._mockDals = new Dictionary<Type, IMockDAL>();

            //_dalFactory.AvailableValues.Returns(
            //    new AvailableValues(
            //        new Range<decimal>(decimal.MinValue, decimal.MaxValue),
            //        new Range<DateTime>(SqlDateTime.MinValue.Value, SqlDateTime.MaxValue.Value)))
            //    .Repeat
            //    .Any();

            this.DefaultInitDals(domainAssemblies);


        }

        private void DefaultInitDals(IEnumerable<Assembly> domainAssemblies)
        {
            var genericMethodDefinition = ((Func<MockDalFactoryProvider<TPersistentDomainObjectBase, TIdent>>)
                                           (this.PrivateInitDal<TPersistentDomainObjectBase>))
                .Method
                .GetGenericMethodDefinition();


            domainAssemblies
                .SelectMany(z => z.GetTypes())
                .Where(z => !z.IsAbstract)
                .Where(z => typeof(TPersistentDomainObjectBase).IsAssignableFrom(z))
                .Foreach(z => genericMethodDefinition.MakeGenericMethod(z).InvokeWithExceptionProcessed(this));
        }
        public MockDalFactoryProvider<TPersistentDomainObjectBase, TIdent> Register<T>(params T[] values) where T : IIdentityObject<TIdent>
        {
            var mockDal = this._mockDals[typeof(T)];
            values.Foreach(z => mockDal.Register(z));
            return this;
        }

        private MockDalFactoryProvider<TPersistentDomainObjectBase, TIdent> PrivateInitDal<T>() where T : class, TPersistentDomainObjectBase
        {
            var mockDal = new MockDAL<T, TIdent>();

            this._mockDals.Add(typeof(T), mockDal);

            return this;
        }

        public void Flush()
        {
            this._mockDals.Select(z => z.Value).Foreach(z => z.Flush());
        }
    }
}
