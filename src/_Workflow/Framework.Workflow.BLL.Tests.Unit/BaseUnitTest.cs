using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Framework.Authorization.BLL;
using Framework.Authorization.BLL.Tests.Unit.Support;
using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.UnitTest.Mock;
using Framework.DomainDriven.UnitTest.Mock.StubProxy;
using Framework.Validation;
using Framework.Workflow.BLL;
using Framework.Workflow.Domain.Definition;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Reflection.Emit;

using Framework.Authorization;

using Microsoft.Extensions.DependencyInjection;

using DomainObjectBase = Framework.Workflow.Domain.DomainObjectBase;
using PersistentDomainObjectBase = Framework.Workflow.Domain.PersistentDomainObjectBase;

namespace Framework.Workflow.BLL.Tests.Unit
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class BaseUnitTest
    {
        public BaseUnitTest()
        {

        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestGetContext()
        {
            var context = this.GetContext();
            var values = context.Logics.Workflow.GetUnsecureQueryable().ToList();
            Assert.IsTrue(values.Any());
        }

        [TestMethod]
        public void TestGetMock()
        {
            Assert.IsNotNull(this.GetMock<PersistentDomainObjectBase>());
        }

        [TestMethod]
        public void TestGetStub()
        {
            Assert.IsNotNull(this.GetStub<PersistentDomainObjectBase>());
        }

        protected IWorkflowBLLContext GetContext()
        {
            var result = Substitute.For<IWorkflowBLLContext>();

            var contextContainer = Substitute.For<IWorkflowBLLContextContainer>();
            contextContainer.Workflow.Returns(result);
            //var parameterParser = Substitute.For<ParameterParserBase>(contextContainer.Workflow);

            var bllFactoryContainer = new WorkflowBLLFactoryContainer(result);
            //var fetchService = Substitute.For<IFetchService<PersistentDomainObjectBase>>();
            result.FetchService.Returns(FetchService<PersistentDomainObjectBase>.Mixed);

            #region Authorization

            var authorization = Substitute.For<IAuthorizationBLLContext>();

            var serviceProvider = new ServiceCollection().AddScoped(_ => authorization)
                                                         .AddScoped(_ => result)
                                                         .Self(WorkflowBLLFactoryContainer.RegisterBLLFactory)
                                                         .BuildServiceProvider();

            authorization.ServiceProvider.Returns(serviceProvider);
            result.ServiceProvider.Returns(serviceProvider);



            //var runAsManager = new AuthorizationRunAsManger(authorization);//Substitute.For<IRunAsManager>();

            var runAsManager = Substitute.For<IRunAsManager>();

            var authSourceListener = new BLLSourceEventListenerContainer<Authorization.Domain.PersistentDomainObjectBase>();
            var authOperationListener = new BLLOperationEventListenerContainer<Authorization.Domain.DomainObjectBase>();

            var dalFactoryAuth = new MockDalFactoryProvider<Authorization.Domain.PersistentDomainObjectBase, Guid>(new List<Assembly> { typeof(BusinessRole).Assembly });
            dalFactoryAuth.Register(new[] { new Authorization.Domain.BusinessRole() });
            authorization.DalFactory.Returns(dalFactoryAuth.DALFactory);

            var authSecurityService = Substitute.For<AuthorizationSecurityService>(authorization);
            authorization.SecurityService.Returns(authSecurityService);

            var logics = Substitute.For<IAuthorizationBLLFactoryContainer>();

            var permissionFactory = new PermissionBLLFactory(authorization);
            var permission = permissionFactory.Create();

            var businessRoleFactory = new BusinessRoleBLLFactory(authorization);
            var businessRole = businessRoleFactory.Create();

            var principalFactory = new PrincipalBLLFactory(authorization);
            var principal = principalFactory.Create();

            var permissionFilterItemFactory = new PermissionFilterItemBLLFactory(authorization);
            var permissionFilterItem = permissionFilterItemFactory.Create();
            var authOperationFactory = new OperationBLLFactory(authorization);
            var authOperation = authOperationFactory.Create();

            logics.BusinessRole.Returns(businessRole);
            logics.Permission.Returns(permission);
            logics.Principal.Returns(principal);
            logics.PermissionFilterItem.Returns(permissionFilterItem);
            logics.Operation.Returns(authOperation);

            authorization.Logics.Returns(logics);
            authorization.RunAsManager.Returns(runAsManager);

            //var authFactoryContainer = new AuthorizationBLLFactoryContainer(authorization);

            var authTestConfiguration = new AuthorizationTestConfiguration();
            var authFactoryContainer = StubProxyFactory.CreateStub(new AuthorizationBLLFactoryContainer(authorization), authTestConfiguration.BLLFactoryConfiguration.InitializeActions);

            ((IBLLFactoryContainerContext<IAuthorizationBLLFactoryContainer>)authorization).Logics.Returns(authFactoryContainer);
            ((IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultSecurityBLLFactory<Authorization.Domain.PersistentDomainObjectBase, AuthorizationSecurityOperationCode, Guid >>>)authorization).Logics.Returns(authFactoryContainer);

            authorization.SourceListeners.Returns(authSourceListener);
            authorization.OperationListeners.Returns(authOperationListener);

            authorization.Validator.Returns(ValidatorBase.Success);

            #endregion

            result.Logics.Returns(bllFactoryContainer);

            contextContainer.Workflow.Returns(result);

            result.Authorization.Returns(authorization);

            ((IBLLFactoryContainerContext<IWorkflowBLLFactoryContainer>)result).Logics.Returns(bllFactoryContainer);
            ((IBLLFactoryContainerContext<IBLLFactoryContainer<IDefaultSecurityBLLFactory<PersistentDomainObjectBase, WorkflowSecurityOperationCode, Guid>>>)result).Logics.Returns(bllFactoryContainer);

                result.SecurityService.Returns(new WorkflowSecurityService(result));
            result.SourceListeners.Returns(new BLLSourceEventListenerContainer<PersistentDomainObjectBase>());
            result.OperationListeners.Returns(new BLLOperationEventListenerContainer<DomainObjectBase>());

            //WorkflowDefaultBLLFactory
            result.Validator
                .Returns(ValidatorBase.Success);

            result.AnonymousTypeBuilder
                .Returns(new AnonymousTypeByFieldBuilder<TypeMap<ParameterizedTypeMapMember>, ParameterizedTypeMapMember>(new AnonymousTypeBuilderStorage("BaseUnitTestAssembly")));

            var dalFactory = new MockDalFactoryProvider<PersistentDomainObjectBase, Guid>(new List<Assembly> { typeof(State).Assembly });
            result.DalFactory.Returns(dalFactory.DALFactory);
            dalFactory.Register(new[] { new Domain.Definition.Workflow() });

            return result;
        }

        protected T GetMock<T>() where T : class
        {
            return Substitute.For<T>();

            //TODO:Migrate
            //var mocks =  new MockRepository();
            //return mocks.PartialMock<T>();
        }

        protected T GetStub<T>() where T : class
        {
            return Substitute.For<T>();
        }
    }
}
