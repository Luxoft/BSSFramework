using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Framework.HierarchicalExpand;
using Framework.QueryableSource;
using Framework.SecuritySystem.Exceptions;
using Framework.SecuritySystem.Rules.Builders;
using V1 = Framework.SecuritySystem.Rules.Builders.MaterializedPermissions;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Xunit;

namespace Framework.SecuritySystem.DiTests
{
    public class MainTests
    {
        private readonly BusinessUnit bu1;
        private readonly BusinessUnit bu2;
        private readonly BusinessUnit bu3;

        private readonly List<Dictionary<Type, List<Guid>>> permissions;

        private readonly Employee employee1;
        private readonly Employee employee2;
        private readonly Employee employee3;

        private readonly IServiceProvider serviceProvider;

        public MainTests()
        {
            this.bu1 = new BusinessUnit() { Id = Guid.NewGuid() };
            this.bu2 = new BusinessUnit() { Id = Guid.NewGuid(), Parent = this.bu1 };
            this.bu3 = new BusinessUnit() { Id = Guid.NewGuid() };

            this.permissions = new List<Dictionary<Type, List<Guid>>>
                               {
                                   new()
                                   {
                                       { typeof(BusinessUnit), new [] { this.bu1.Id }.ToList() }
                                   }
                               };


            this.employee1 = new Employee() { Id = Guid.NewGuid(), BusinessUnit = this.bu1 };
            this.employee2 = new Employee() { Id = Guid.NewGuid(), BusinessUnit = this.bu2 };
            this.employee3 = new Employee() { Id = Guid.NewGuid(), BusinessUnit = this.bu3 };

            this.serviceProvider = this.BuildServiceProvider();
        }

        [Fact]
        public void TestEmployeesSecurity_EmployeeHasAccessCorrect()
        {
            // Arrange

            // Act
            var employeeDomainSecurityService = this.serviceProvider.GetRequiredService<IDomainSecurityService<Employee, ExampleSecurityOperation>>();
            var provider = employeeDomainSecurityService.GetSecurityProvider(BLLSecurityMode.View);

            // Assert
            var result1 = provider.HasAccess(this.employee1);
            var result2 = provider.HasAccess(this.employee2);
            var result3 = provider.HasAccess(this.employee3);

            result1.Should().BeTrue();
            result2.Should().BeTrue();
            result3.Should().BeFalse();
        }

        [Fact]
        public void CheckEmployeeWithoutSecurity_ExceptionRaised()
        {
            // Arrange

            // Act
            var employeeDomainSecurityService = this.serviceProvider.GetRequiredService<IDomainSecurityService<Employee, ExampleSecurityOperation>>();
            var provider = employeeDomainSecurityService.GetSecurityProvider(BLLSecurityMode.View);

            // Assert
            Action checkAccessAction = () => provider.CheckAccess(this.employee3);

            checkAccessAction.Should().Throw<AccessDeniedException<Guid>>();
        }


        private IServiceProvider BuildServiceProvider()
        {
            return new ServiceCollection()

                   .AddScoped(_ => this.BuildQueryableSource())
                   .AddScoped<IPrincipalPermissionSource<Guid>>(_ => new ExamplePrincipalPermissionSource(this.permissions))

                   .AddScoped<IDisabledSecurityProviderContainer<PersistentDomainObjectBase>, DisabledSecurityProviderContainer<PersistentDomainObjectBase>>()
                   .AddScoped<IAccessDeniedExceptionService<PersistentDomainObjectBase>, AccessDeniedExceptionService<PersistentDomainObjectBase, Guid>>()
                   .AddScoped<ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>, V1.SecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>>()
                   .AddScoped<IAuthorizationSystem<Guid>, ExampleAuthorizationSystem>()
                   .AddScoped<IHierarchicalObjectExpanderFactory<Guid>, HierarchicalObjectExpanderFactory<PersistentDomainObjectBase, Guid>>()
                   .AddScoped<IDomainSecurityService<Employee, ExampleSecurityOperation>, EmployeeSecurityService>()
                   .AddScoped<ISecurityOperationResolver<PersistentDomainObjectBase, ExampleSecurityOperation>, ExampleSecurityOperationResolver>()
                   .AddScoped<IHierarchicalRealTypeResolver, IdentityHierarchicalRealTypeResolver>()

                   .BuildServiceProvider();
        }

        private IQueryableSource<PersistentDomainObjectBase> BuildQueryableSource()
        {
            var queryableSource = Substitute.For<IQueryableSource<PersistentDomainObjectBase>>();

            queryableSource.GetQueryable<BusinessUnit>().Returns(new[] { this.bu1, this.bu2, this.bu3 }.AsQueryable());

            queryableSource.GetQueryable<Employee>().Returns(new[] { this.employee1, this.employee2, this.employee3 }.AsQueryable());

            return queryableSource;
        }
    }
}
