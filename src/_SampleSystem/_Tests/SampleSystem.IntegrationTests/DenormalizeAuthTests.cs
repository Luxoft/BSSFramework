using System;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData;

using BusinessRole = SampleSystem.IntegrationTests.__Support.Utils.BusinessRole;

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

        //[TestMethod]
        //public void TestInlineEval_TestPassed()
        //{
        //    // Arrange

        //    // Act
        //    var objIdents = this.GetContextEvaluator().Evaluate(DBSessionMode.Read, TestEmployeeLogin, ctx =>
        //    {
        //        var baseFilter = BuildTestPlainAuthObjectSecurityFilter(ctx, SampleSystemSecurityOperation.EmployeeView);
        //        var filter = baseFilter.ExpandConst().InlineEval();

        //        var objs = ctx.Logics.TestPlainAuthObject.GetUnsecureQueryable().Where(filter).ToList();

        //        return objs.ToList(obj => obj.ToIdentityDTO());
        //    });

        //    // Assert
        //    objIdents.Count().Should().Be(1);
        //    objIdents[0].Should().Be(this.testPlainAuthObjectIdent);
        //}

        //[TestMethod]
        //public void AdminTestInlineEval_TestPassed()
        //{
        //    // Arrange

        //    // Act
        //    var objIdents = this.GetContextEvaluator().Evaluate(DBSessionMode.Read, ctx =>
        //    {
        //        var baseFilter = BuildTestPlainAuthObjectSecurityFilter(ctx, SampleSystemSecurityOperation.EmployeeView);
        //        var filter = baseFilter.ExpandConst().InlineEval();

        //        var objs = ctx.Logics.TestPlainAuthObject.GetUnsecureQueryable().Where(filter).ToList();

        //        return objs.ToList(obj => obj.ToIdentityDTO());
        //    });

        //    // Assert
        //    objIdents.Count().Should().Be(2);
        //}

        //private static Expression<Func<TestPlainAuthObject, bool>> BuildTestPlainAuthObjectSecurityFilter(ISampleSystemBLLContext context, ContextSecurityOperation<SampleSystemSecurityOperationCode> securityOperation)
        //{
        //    var authContext = context.Authorization;

        //    var buFilter = BuildBuFilterExpression(context, securityOperation.SecurityExpandType);

        //   // var employeeFilter = BuildEmployeeFilterExpression(context, securityOperation.SecurityExpandType);

        //    return testPlainAuthObject =>
        //                   authContext.GetPermissionQuery(securityOperation)
        //                              .Any(permission =>
        //                                        permission.DenormalizedItems.Any(permissionFilterItem =>
        //                                            buFilter.Eval(testPlainAuthObject, permission, permissionFilterItem)
        //                                         //&& employeeFilter.Eval(testPlainAuthObject, permission, permissionFilterItem)
        //                                            ));

        //}

        //private static Expression<Func<TestPlainAuthObject, IPermission<Guid>, IDenormalizedPermissionItem<Guid>, bool>> BuildBuFilterExpression(ISampleSystemBLLContext context, HierarchicalExpandType expandType)
        //{
        //    var entityTypeId = context.Authorization.ResolveSecurityTypeId(typeof(BusinessUnit));

        //    var eqIdentsExpr = ExpressionHelper.GetEquality<Guid>();

        //    var grandIdent = context.Authorization.GrandAccessIdent;

        //    var expander = context.HierarchicalObjectExpanderFactory.CreateQuery(typeof(BusinessUnit));

        //    var expandExpression = expander.TryGetSingleExpandExpression(expandType);

        //    var containsItem = (expandExpression == null
        //                                ? ExpressionHelper.Create((Guid itemId, Guid permissionEntityId) => eqIdentsExpr.Eval(itemId, permissionEntityId))
        //                                : ExpressionHelper.Create((Guid itemId, Guid permissionEntityId) => expandExpression.Eval(permissionEntityId).Contains(itemId)))
        //            .ExpandConst()
        //            .InlineEval();

        //    Expression<Func<TestPlainAuthObject, IPermission<Guid>, IDenormalizedPermissionItem<Guid>, bool>> baseFilter = (domainObject, _, denormalizedPermissionItem) =>

        //                   eqIdentsExpr.Eval(denormalizedPermissionItem.EntityType.Id, entityTypeId)

        //                   && (eqIdentsExpr.Eval(denormalizedPermissionItem.EntityId, grandIdent)

        //                       || !domainObject.Items.Select(item => item.BusinessUnit).Any()

        //                       || domainObject.Items.Select(item => item.BusinessUnit).Any(item => containsItem.Eval(item.Id, denormalizedPermissionItem.EntityId)));

        //    return baseFilter.ExpandConst().InlineEval();
        //}

        //private static Expression<Func<TestPlainAuthObject, IPermission<Guid>, IDenormalizedPermissionItem<Guid>, bool>> BuildEmployeeFilterExpression(ISampleSystemBLLContext context, HierarchicalExpandType expandType)
        //{
        //    var entityTypeId = context.Authorization.ResolveSecurityTypeId(typeof(Employee));

        //    var eqIdentsExpr = ExpressionHelper.GetEquality<Guid>();

        //    var getIdents = ExpressionHelper.Create((IPermission<Guid> permission) =>
        //                                                    permission.DenormalizedItems
        //                                                              .Where(item => eqIdentsExpr.Eval(item.EntityType.Id, entityTypeId))
        //                                                              .Select(pfe => pfe.EntityId))
        //                                    .ExpandConst()
        //                                    .InlineEval();

        //    var grandIdent = context.Authorization.GrandAccessIdent;

        //    var hasGrandAccess = ExpressionHelper.Create((IPermission<Guid> permission) => getIdents.Eval(permission).Any(entityId => eqIdentsExpr.Eval(entityId, grandIdent)))
        //                                         .ExpandConst()
        //                                         .InlineEval();

        //    var expander = context.HierarchicalObjectExpanderFactory.CreateQuery(typeof(Employee));

        //    var expandExpression = expander.GetExpandExpression(expandType);

        //    var expandExpressionQ = from idents in getIdents
        //                            select expandExpression.Eval(idents);

        //    return (domainObject, permission, denormalizedPermissionItem) =>

        //                   hasGrandAccess.Eval(permission)

        //                   || domainObject.Employee == null

        //                   || expandExpressionQ.Eval(permission).Contains(domainObject.Employee.Id);
        //}
    }
}
