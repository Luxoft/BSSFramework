using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.NHibernate;
using Framework.HierarchicalExpand;
using Framework.Security;
using Framework.SecuritySystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;

using BusinessRole = SampleSystem.IntegrationTests.__Support.Utils.BusinessRole;
using Expression = System.Linq.Expressions.Expression;
using LambdaExpression = System.Linq.Expressions.LambdaExpression;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class DenormalizeAuthTests : TestBase
    {
        private const string TestEmployeeLogin = "DS SecurityTester";

        private LocationIdentityDTO loc1Ident;

        private BusinessUnitIdentityDTO bu1Ident;

        private ManagementUnitIdentityDTO mbu1Ident;

        private BusinessUnitIdentityDTO bu2Ident;


        private TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdent;


        [TestInitialize]
        public void SetUp()
        {

            this.loc1Ident = this.DataHelper.SaveLocation();

            this.bu1Ident = this.DataHelper.SaveBusinessUnit();

            this.bu2Ident = this.DataHelper.SaveBusinessUnit();

            this.mbu1Ident = this.DataHelper.SaveManagementUnit();

            this.DataHelper.SaveEmployee(login: TestEmployeeLogin);

            this.AuthHelper.SetUserRole(TestEmployeeLogin, new SampleSystemPermission(BusinessRole.Administrator, this.bu1Ident, this.mbu1Ident, this.loc1Ident));

            this.AuthHelper.SetUserRole(TestEmployeeLogin, new SampleSystemPermission(BusinessRole.Administrator, this.bu1Ident, null, this.loc1Ident));

            this.EvaluateWrite(
                               context =>
                               {
                                   var sampleObj = new TestPlainAuthObject
                                   {
                                           Name = "obj1",
                                           Location = context.Logics.Location.GetById(this.loc1Ident.Id, true),
                                   };

                                   new TestItemAuthObject(sampleObj)
                                   {
                                           BusinessUnit = context.Logics.BusinessUnit.GetById(this.bu1Ident.Id, true),
                                           ManagementUnit = context.Logics.ManagementUnit.GetById(this.mbu1Ident.Id, true)
                                   };

                                   context.Logics.TestPlainAuthObject.Save(sampleObj);

                                   this.testPlainAuthObjectIdent = sampleObj.ToIdentityDTO();
                               });

            this.EvaluateWrite(
                               context =>
                               {
                                   var sampleObj = new TestPlainAuthObject
                                                   {
                                                           Name = "obj2",
                                                           Location = context.Logics.Location.GetById(this.loc1Ident.Id, true),
                                                   };

                                   new TestItemAuthObject(sampleObj)
                                   {
                                           BusinessUnit = context.Logics.BusinessUnit.GetById(this.bu2Ident.Id, true),
                                           ManagementUnit = context.Logics.ManagementUnit.GetById(this.mbu1Ident.Id, true)
                                   };

                                   context.Logics.TestPlainAuthObject.Save(sampleObj);
                               });
        }

        [TestMethod]
        public void Test_AAA()
        {
            var objIdents = this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, TestEmployeeLogin, ctx =>
                                                                                                                   {
                                                                                                                       var baseFilter = BuildTestPlainAuthObjectSecurityFilter(ctx, SampleSystemSecurityOperation.EmployeeView);
                                                                                                                       var filter = baseFilter.ExpandConst().ExpandEval();

                                                                                                                       var objs = ctx.Logics.TestPlainAuthObject.GetUnsecureQueryable().Where(filter).ToList();

                                                                                                                       return objs.ToList(obj => obj.ToIdentityDTO());
                                                                                                                   });

            objIdents.Count().Should().Be(1);
            objIdents[0].Should().Be(this.testPlainAuthObjectIdent);
        }

        private static Expression<Func<TestPlainAuthObject, bool>> BuildTestPlainAuthObjectSecurityFilter(ISampleSystemBLLContext context, ContextSecurityOperation<SampleSystemSecurityOperationCode> securityOperation)
        {
            var authContext = context.Authorization;

            var entityTypeDict = authContext.Logics.EntityType.GetFullList().ToDictionary(et => et.Name, et => et.Id);

            var buFilter = BuildBuFilterExpression(context, entityTypeDict, securityOperation.SecurityExpandType);

            return testPlainAuthObject =>
                           authContext.GetPermissionQuery(securityOperation)
                                      .Any(permission => buFilter.Eval(testPlainAuthObject, permission));

        }

        private static Expression<Func<TestPlainAuthObject, IPermission<Guid>, bool>> BuildBuFilterExpression(ISampleSystemBLLContext context, IReadOnlyDictionary<string, Guid> entityTypeDict, HierarchicalExpandType expandType)
        {
            var entityTypeId = context.Authorization.ResolveSecurityTypeId(typeof(BusinessUnit));

            var eqIdentsExpr = ExpressionHelper.GetEquality<Guid>();

            var getIdents = ExpressionHelper.Create((IPermission<Guid> permission) =>
                                                            permission.FilterItems
                                                                      .Select(fi => fi.Entity)
                                                                      .Where(item => eqIdentsExpr.Eval(item.EntityType.Id, entityTypeId))
                                                                      .Select(pfe => pfe.EntityId))
                                            .ExpandConst()
                                            .ExpandEval();

            var expander = (IHierarchicalObjectQueryableExpander<Guid>)context.HierarchicalObjectExpanderFactory.Create(typeof(BusinessUnit));

            var expandExpression = expander.GetExpandExpression(expandType);

            var expandExpressionQ = from idents in getIdents
                                    select expandExpression.Eval(idents);

            return (domainObject, permission) =>

                           !getIdents.Eval(permission).Any()

                           || !domainObject.Items.Select(item => item.BusinessUnit).Any()

                           || domainObject.Items.Select(item => item.BusinessUnit).Any(item => expandExpressionQ.Eval(permission).Contains(item.Id));
        }
    }
}
