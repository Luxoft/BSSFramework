using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.NHibernate;
using Framework.Security;
using Framework.SecuritySystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
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


        private TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdent;


        [TestInitialize]
        public void SetUp()
        {

            this.loc1Ident = this.DataHelper.SaveLocation();

            this.bu1Ident = this.DataHelper.SaveBusinessUnit();

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
        }

        [TestMethod]
        public void Test_AAA()
        {
            var objIdents = this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, TestEmployeeLogin, ctx =>
            {
                var filter = BuildTestPlainAuthObjectSecurityFilter(ctx, SampleSystemSecurityOperation.EmployeeView);

                var objs = ctx.Logics.TestPlainAuthObject.GetUnsecureQueryable().Where(filter).ToList();

                return objs.ToList(obj => obj.ToIdentityDTO());
            });

            objIdents.Count().Should().Be(1);
            objIdents[0].Should().Be(this.testPlainAuthObjectIdent);
        }

        private static Expression<Func<TestPlainAuthObject, bool>> BuildTestPlainAuthObjectSecurityFilter(ISampleSystemBLLContext context, SecurityOperation<SampleSystemSecurityOperationCode> securityOperation)
        {
            var operationId = securityOperation.Code.GetSecurityOperationAttribute().Guid;

            var authContext = context.Authorization;

            var permissionQuery = authContext.Logics.Permission.GetUnsecureQueryable();

            var buQuery = context.Logics.Default.Create<BusinessUnit>().GetUnsecureQueryable();

            var buAncestorQuery = context.Logics.Default.Create<BusinessUnitAncestorLink>().GetUnsecureQueryable();

            var today = context.DateTimeService.Today;

            var entityTypeDict = authContext.Logics.EntityType.GetFullList().ToDictionary(et => et.Name, et => et.Id);

            return testPlainAuthObject =>

                           (from permission in permissionQuery

                            where permission.Principal == authContext.CurrentPrincipal

                            where permission.Role.BusinessRoleOperationLinks.Any(
                             link => link.Operation.Id == operationId)

                            where permission.Period.Contains(today)

                            where permission.Status == PermissionStatus.Approved

                            from permissionBuId in permission.DenormalizedItems.Where(item => item.EntityType.Id == entityTypeDict[nameof(BusinessUnit)]).Select(pfe => pfe.EntityId)

                            let availableBusinessUnitIdents = from permissionBuAncestor in buAncestorQuery

                                                              where permissionBuAncestor.Ancestor.Id == permissionBuId

                                                              select permissionBuAncestor.Child.Id

                            let domainObjectBusinessUnits = testPlainAuthObject.Items.Select(item => item.BusinessUnit.Id)

                            ////Any
                            //where permissionBuId == DenormalizedPermissionItem.GrandAccessGuid
                            //      || !domainObjectBusinessUnits.Any()
                            //      || domainObjectBusinessUnits.Any(bu => availableBusinessUnitIdents.Contains(bu))

                            ////AnyStrictly
                            //where permissionBuId == DenormalizedPermissionItem.GrandAccessGuid
                            //      || domainObjectBusinessUnits.Any(buId => availableBusinessUnitIdents.Contains(buId))

                            //All
                            where permissionBuId == DenormalizedPermissionItem.GrandAccessGuid
                               || domainObjectBusinessUnits.All(buId => availableBusinessUnitIdents.Contains(buId))

                            select permission).Any();
        }

        //private static Expression<Func<TestPlainAuthObject, bool>> BuildTestPlainAuthObjectSecurityFilter(ISampleSystemBLLContext context, SecurityOperation<SampleSystemSecurityOperationCode> securityOperations)
        //{
        //    var operationId = securityOperations.Code.GetSecurityOperationAttribute().Guid;

        //    var authContext = context.Authorization;

        //    var permissionQuery = authContext.Logics.Permission.GetUnsecureQueryable();

        //    var buQuery = context.Logics.Default.Create<BusinessUnit>().GetUnsecureQueryable();

        //    var buAncestorQuery = context.Logics.Default.Create<BusinessUnitAncestorLink>().GetUnsecureQueryable();

        //    var today = context.DateTimeService.Today;

        //    var entityTypeDict = authContext.Logics.EntityType.GetFullList().ToDictionary(et => et.Name, et => et.Id);

        //    return testPlainAuthObject =>

        //                   (from permission in permissionQuery

        //                    where permission.Principal == authContext.CurrentPrincipal

        //                    where permission.Role.BusinessRoleOperationLinks.Any(
        //                     link => link.Operation.Id == operationId)

        //                    where permission.Period.Contains(today)

        //                    where permission.Status == PermissionStatus.Approved

        //                    let baseAvailableBusinessUnitIdents = permission.FilterItems.Where(pfe => pfe.EntityType.Id == entityTypeDict[nameof(BusinessUnit)]).Select(pfi => pfi.Entity.EntityId)

        //                    let availableBusinessUnitIdents = from permissionBuId in permission.FilterItems.Where(pfe => pfe.EntityType.Id == entityTypeDict[nameof(BusinessUnit)]).Select(pfi => pfi.Entity.EntityId)

        //                                                      from permissionBuAncestor in buAncestorQuery

        //                                                      where permissionBuAncestor.Ancestor.Id == permissionBuId

        //                                                      select permissionBuAncestor.Child.Id

        //                    //Any
        //                    let domainObjectBusinessUnits = testPlainAuthObject.Items.Select(item => item.BusinessUnit.Id)

        //                    where !baseAvailableBusinessUnitIdents.Any()
        //                          || !domainObjectBusinessUnits.Any()
        //                          || domainObjectBusinessUnits.Any(bu => availableBusinessUnitIdents.Contains(bu))

        //                    //AnyStrictly
        //                    //let domainObjectBusinessUnits = testPlainAuthObject.Items.Select(item => item.BusinessUnit.Id)
        //                    //where domainObjectBusinessUnits.Any(buId => !availableBusinessUnitIdents.Any() || availableBusinessUnitIdents.Contains(buId))

        //                    //All
        //                    //let domainObjectBusinessUnits = testPlainAuthObject.Items.Select(item => item.BusinessUnit.Id)
        //                    //where domainObjectBusinessUnits.All(buId => !availableBusinessUnitIdents.Any() || availableBusinessUnitIdents2.Contains(buId))
        //                      //|| !domainObjectBusinessUnits.Any()


        //                    //&& (locations.Contains(testPlainAuthObject.Location.Id))


        //                    select permission).Any();
        //}
    }
}
