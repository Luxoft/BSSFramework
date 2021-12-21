using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using NSubstitute;

namespace Framework.DomainDriven.UnitTest.Mock
{
    public class MockDalFactoryProvider<TPersistentDomainObjectBase, TIdent> where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        private readonly IDictionary<Type, IMockDAL> _mockDals;
        private readonly IDALFactory<TPersistentDomainObjectBase, TIdent> _dalFactory;

        public MockDalFactoryProvider(IEnumerable<Assembly> domainAssemblies)
        {
            this._mockDals = new Dictionary<Type, IMockDAL>();

            var fff = Substitute.For<IDALFactory>();
            this._dalFactory = Substitute.For<IDALFactory<TPersistentDomainObjectBase, TIdent>>();
            //_dalFactory.AvailableValues.Returns(
            //    new AvailableValues(
            //        new Range<decimal>(decimal.MinValue, decimal.MaxValue),
            //        new Range<DateTime>(SqlDateTime.MinValue.Value, SqlDateTime.MaxValue.Value)))
            //    .Repeat
            //    .Any();

            this.DefaultInitDals(domainAssemblies);


        }

        public IDALFactory<TPersistentDomainObjectBase, TIdent> DALFactory
        {
            get { return this._dalFactory; }
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
            var mockDal = this._mockDals[typeof (T)];
            values.Foreach(z => mockDal.Register(z));
            return this;
        }

        private MockDalFactoryProvider<TPersistentDomainObjectBase, TIdent> PrivateInitDal<T>() where T : class, TPersistentDomainObjectBase
        {
            var mockDal = new MockDAL<T, TIdent>();

            this._dalFactory.CreateDAL<T>().Returns(mockDal);

            this._mockDals.Add(typeof(T), mockDal);

            return this;
        }

        public void Flush()
        {
            this._mockDals.Select(z=>z.Value).Foreach(z=>z.Flush());
        }
    }
}