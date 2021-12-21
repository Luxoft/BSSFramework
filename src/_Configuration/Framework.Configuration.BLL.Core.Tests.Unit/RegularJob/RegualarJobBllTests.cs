using System;
using System.Linq;

using FluentAssertions;

using Framework.Authorization.BLL;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;
using Framework.UnitTesting;
using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using NSubstitute;

namespace Framework.Configuration.BLL.Core.Tests.Unit.RegularJob
{
    [TestFixture]
    public class RegualarJobBllTests : TestFixtureBase
    {
        private IRegularJobBLL bll;

        private IDAL<Domain.RegularJob, Guid> dal;

        [SetUp]
        public void SetUp()
        {
            var authorizationBLLContext = this.Fixture.RegisterStub<IAuthorizationBLLContext>();
            var configurationBLLContext = this.Fixture.RegisterStub<IConfigurationBLLContext>();
            var dateTimeService = this.Fixture.RegisterStub<IDateTimeService>();
            var messageSender = this.Fixture.RegisterStub<IMessageSender<RunRegularJobModel>>();

            var validator = this.Fixture.RegisterStub<IValidator>();
            validator.GetValidationResult(Arg.Any<Domain.RegularJob>(), Arg.Any<int>(), Arg.Any<IValidationState>())
                     .Returns(ValidationResult.Success);

            dateTimeService.Now.Returns(DateTime.Now);
            dateTimeService.Today.Returns(DateTime.Today);

            var queryProvider = this.Fixture.RegisterStub<IQueryProvider>();
            var queryable = this.Fixture.RegisterStub<IQueryable<Domain.RegularJob>>();
            queryable.Provider.Returns(queryProvider);

            this.dal = this.Fixture.RegisterStub<IDAL<Domain.RegularJob, Guid>>();

            var dalFactory = this.Fixture.RegisterStub<IDALFactory<PersistentDomainObjectBase, Guid>>();
            dalFactory.CreateDAL<Domain.RegularJob>().Returns(this.dal);

            var logics = this.Fixture.RegisterStub<IConfigurationBLLFactoryContainer>();
            var namedLock = this.Fixture.RegisterStub<INamedLockBLL>();
            logics.NamedLock.Returns(namedLock);

            var securityProvider = this.Fixture.RegisterStub<ISecurityProvider<Domain.RegularJob>>();
            securityProvider.HasAccess(Arg.Any<Domain.RegularJob>()).Returns(true);

            var securityService = this.Fixture.RegisterStub<IConfigurationSecurityService>();
            securityService.GetSecurityProvider<Domain.RegularJob>(BLLSecurityMode.Disabled).Returns(securityProvider);

            configurationBLLContext.Authorization.Returns(authorizationBLLContext);
            configurationBLLContext.SecurityService.Returns(securityService);
            configurationBLLContext.DalFactory.Returns(dalFactory);
            configurationBLLContext.Logics.Returns(logics);
            configurationBLLContext.DateTimeService.Returns(dateTimeService);
            configurationBLLContext.RegularJobMessageSender.Returns(messageSender);
            configurationBLLContext.Validator.Returns(validator);
            configurationBLLContext.SourceListeners.Returns(new BLLSourceEventListenerContainer<PersistentDomainObjectBase>());
            configurationBLLContext.OperationListeners.Returns(new BLLOperationEventListenerContainer<DomainObjectBase>());

            var serviceProvider = new ServiceCollection()
                                  .AddScoped(_ => configurationBLLContext)
                                  .Self(ConfigurationBLLFactoryContainer.RegisterBLLFactory)
                                  .Self(ConfigurationSecurityServiceBase.Register)
                                  .BuildServiceProvider();

            configurationBLLContext.ServiceProvider.Returns(serviceProvider);

            var factory = new RegularJobBLLFactory(configurationBLLContext);

            this.bll = factory.Create();
        }

        /// <summary>
        /// IADFRAME-444 RegularJob с active=0 запускается
        /// </summary>
        [Test]
        public void Pulse_DontRunInactiveJobs_ActiveJobChangedStatusInactiveNotChanged()
        {
            // Arrange
            var activeJob = new Domain.RegularJob { Active = true };
            var inactiveJob = new Domain.RegularJob { Active = false };

            this.dal.GetQueryable(LockRole.None, null).Returns(new EnumerableQuery<Domain.RegularJob>(new[] { activeJob, inactiveJob }));

            // Act
            this.bll.Pulse();

            // Assert
            activeJob.State.Should().Be(RegularJobState.SendToProcessing);
            inactiveJob.State.Should().Be(RegularJobState.WaitPulse);
        }
    }
}
