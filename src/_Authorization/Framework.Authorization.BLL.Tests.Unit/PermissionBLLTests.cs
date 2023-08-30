using Framework.Authorization.Domain;

using NUnit.Framework;

namespace Framework.Authorization.BLL.Tests.Unit;

[TestFixture]
public class PermissionBLLTests : TestBase
{
    private EntityType entityType;
    private Principal principalTest1;

    private Guid luxTravelSEApproverId;
    private Guid boeingId;
    private PermissionFilterEntity permissionFilterEntityBoeing;
    private BusinessRole businessRoleLuxTravelSEApprover;

    private Guid seManagerId;
    private BusinessRole businessRoleSEManager;

    private Guid businessUnitChangeViewOperationId;
    private Operation businessUnitChangeViewOperation;
    private Guid ubsId;
    private PermissionFilterEntity permissionFilterEntityUbs;

    [SetUp]
    public void Initialize()
    {
        this.luxTravelSEApproverId = Guid.Parse("f6a6b88c-64a5-4331-9a45-a10a0116cb2f");
        this.seManagerId = Guid.Parse("8F69215E-68FA-4AC3-82F2-1F97A47CC473");
        this.boeingId = Guid.Parse("3935a661-e7be-4aa9-a21d-20da94c9f239");
        this.ubsId = Guid.Parse("3B4A8AC9-5054-4C8B-BB08-53CC69A4E35D");
        Guid.Parse("a893dfa6-830e-44bc-8887-c98721005156");
        this.businessUnitChangeViewOperationId = Guid.Parse("7e9516b6-059f-4ec5-817f-f9ce9faacdc4");

        this.businessUnitChangeViewOperation = new Operation { Name = "BusinessUnitChangeView", Id = this.businessUnitChangeViewOperationId };
        this.entityType = new EntityType(true, true) { Name = "BusinessUnit" };
        this.principalTest1 = new Principal { Name = "test1" };
        this.permissionFilterEntityBoeing = new PermissionFilterEntity { EntityType = this.entityType, EntityId = this.boeingId };
        this.permissionFilterEntityUbs = new PermissionFilterEntity { EntityType = this.entityType, EntityId = this.ubsId };

        this.businessRoleLuxTravelSEApprover = new BusinessRole { Name = "LuxTravel SE Approver", Id = this.luxTravelSEApproverId };
        this.businessRoleSEManager = new BusinessRole { Name = "SE Manager", Id = this.seManagerId };

        // ReSharper disable once ObjectCreationAsStatement
        new BusinessRoleOperationLink(this.businessRoleSEManager)
        {
                Operation = this.businessUnitChangeViewOperation
        };

        this.RegisterDomainObject(this.businessUnitChangeViewOperation);
        this.RegisterDomainObject(this.businessRoleLuxTravelSEApprover);
        this.RegisterDomainObject(this.businessRoleSEManager);
        this.RegisterDomainObject(this.permissionFilterEntityBoeing);
        this.RegisterDomainObject(this.permissionFilterEntityUbs);
        this.RegisterDomainObject(this.entityType);
        this.RegisterDomainObject(this.principalTest1);
    }

    [TearDown]
    public void Cleanup()
    {
        this.businessUnitChangeViewOperation = null;
        this.entityType = null;
        this.principalTest1 = null;
        this.permissionFilterEntityBoeing = null;
        this.permissionFilterEntityUbs = null;
        this.businessRoleLuxTravelSEApprover = null;
        this.businessRoleSEManager = null;
    }

    [Test]
    public void GetFullList_ReturnAll()
    {
        // Arrange
        var permission = new Permission(new Principal());
        this.RegisterDomainObject(permission);

        // Act
        var zz = this.Context.Logics.Permission.GetFullList();

        // Assert
        Assert.AreEqual(permission, zz.Single());
    }
}
