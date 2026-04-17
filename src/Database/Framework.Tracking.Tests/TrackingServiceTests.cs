using Framework.Database;
using NSubstitute;

namespace Framework.Tracking.Tests;

public class TrackingServiceTests
{
    private IObjectStateService objectStateService;

    private TrackingService<PersistentDomainObject> trackingService;

    private readonly IPersistentInfoService persistentInfoService = new PersistentInfoService();

    public TrackingServiceTests()
    {
        this.objectStateService = Substitute.For<IObjectStateService>();
        this.trackingService = new TrackingService<PersistentDomainObject>(this.objectStateService, this.persistentInfoService);
    }

    [Fact]
    public void GetPersistentState_IsNewTrue_ReturnNotPersistent()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);

        // Act
        var state = this.trackingService.GetPersistentState(domainObject);

        // Assert
        Assert.Equal(PersistentLifeObjectState.NotPersistent, state);
    }

    [Fact]
    public void GetPersistentState_IsNewFalseIsRemovingFalse_ReturnPersistent()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);

        // Act
        var state = this.trackingService.GetPersistentState(domainObject);

        // Assert
        Assert.Equal(PersistentLifeObjectState.Persistent, state);
    }

    [Fact]
    public void GetPersistentState_IsNewFalseIsRemovingTrue_ReturnMarkAsRemoved()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(true);

        // Act
        var state = this.trackingService.GetPersistentState(domainObject);

        // Assert
        Assert.Equal(PersistentLifeObjectState.MarkAsRemoved, state);
    }

    [Fact]
    public void GetChanges_IsNewFalseIsRemovingFalseNoChanges_ResultShouldBeEmpty()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        this.objectStateService.GetModifiedObjectStates(domainObject).Returns([]);

        // Act
        var result = this.trackingService.GetChanges(domainObject);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetChanges_IsNewFalseIsRemovingFalseOneChange_ResultShouldHaveCountOne()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        var change = new ObjectState("Name", "2", "1", true);
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        this.objectStateService.GetModifiedObjectStates(domainObject).Returns([change]);

        // Act
        var result = this.trackingService.GetChanges(domainObject);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public void GetChanges_IsNewFalseIsRemovingFalseSeveralChanges_ResultShouldHaveAsManyChangesAsGetModifiedObjectStatesReturned()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        var change1 = new ObjectState("Name", "2", "1", true);
        var change2 = new ObjectState("Id", "2", "1", true);
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var objectStates = new[] { change1, change2 };
        this.objectStateService.GetModifiedObjectStates(domainObject).Returns(objectStates);

        // Act
        var result = this.trackingService.GetChanges(domainObject);

        // Assert
        Assert.Equal(objectStates.Length, result.Count());
    }

    [Fact]
    public void GetChanges_IsNewTrue_ResultShouldHaveAllPropertiesChanges()
    {
        // Arrange
        var domainObject = new PersistentDomainObject() { Id = Guid.NewGuid(), CreatedAt = DateTime.Today, Name = "z", Array = [], ModifiedAt = DateTime.Today };
        this.objectStateService.IsNew(domainObject).Returns(true);
        var allProperties = typeof(PersistentDomainObject).GetProperties();

        // Act
        var result = this.trackingService.GetChanges(domainObject);

        // Assert
        Assert.Equal(allProperties.Length, result.Count());
    }

    [Fact]
    public void GetChanges_NewModeIsNewTrueNoPropertiesSet_ResultShouldHaveZeroChanges()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);

        // Act
        var result = this.trackingService.GetChanges(domainObject, GetChangesMode.IgnoreDefaultValues);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetChanges_DefaultModeIsNewTrueNoPropertiesSet_ResultShouldHaveZeroChanges()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);

        // Act
        var result = this.trackingService.GetChanges(domainObject);

        // Assert
        Assert.Equal(5, result.Count());
    }

    [Fact]
    public void GetChangingState_IsNewTrue_ReturnChanging()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);

        // Act
        var result = this.trackingService.GetChangingState(domainObject);

        // Assert
        Assert.Equal(ChangingLifeObjectState.Changing, result);
    }

    [Fact]
    public void GetChangingState_IsNewFalseHasChanges_ReturnChanging()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        var change = new ObjectState("Name", "2", "1", true);
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        this.objectStateService.GetModifiedObjectStates(domainObject).Returns([change]);

        // Act
        var result = this.trackingService.GetChangingState(domainObject);

        // Assert
        Assert.Equal(ChangingLifeObjectState.Changing, result);
    }

    [Fact]
    public void GetChangingState_IsNewFalseNoChanges_ReturnOriginal()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        this.objectStateService.GetModifiedObjectStates(domainObject).Returns([]);

        // Act
        var result = this.trackingService.GetChangingState(domainObject);

        // Assert
        Assert.Equal(ChangingLifeObjectState.Original, result);
    }

    [Fact]
    public void GetPrevValue_IsNewFalseNoChangesNoPrevValue_ReturnDefault()
    {
        // Arrange
        const string CurrName = "2";
        const string DefaultName = "x";
        var domainObject = new PersistentDomainObject { Name = CurrName };
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        this.objectStateService.GetModifiedObjectStates(domainObject).Returns([]);

        // Act
        var result = this.trackingService.GetPrevValue(domainObject, x => x.Name, DefaultName);

        // Assert
        Assert.Equal(DefaultName, result);
    }

    [Fact]
    public void GetPrevValue_IsNewFalseHasChangesHasPrevValue_ReturnPrevValue()
    {
        // Arrange
        const string DefaultName = "x";
        const string CurrName = "2";
        const string PrevName = "1";
        var domainObject = new PersistentDomainObject { Name = CurrName };
        var change = new ObjectState("Name", CurrName, PrevName, true);
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        this.objectStateService.GetModifiedObjectStates(domainObject).Returns([change]);

        // Act
        var result = this.trackingService.GetPrevValue(domainObject, x => x.Name, DefaultName);

        // Assert
        Assert.Equal(PrevName, result);
    }

    [Fact]
    public void GetPrevOrCurrentValue_IsNewFalseNoChangesNoPrevValue_ReturnCurrentValue()
    {
        // Arrange
        const string CurrName = "2";
        var domainObject = new PersistentDomainObject { Name = CurrName };
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        this.objectStateService.GetModifiedObjectStates(domainObject).Returns([]);

        // Act
        var result = this.trackingService.GetPrevOrCurrentValue(domainObject, x => x.Name);

        // Assert
        Assert.Equal(CurrName, result);
    }

    [Fact]
    public void GetPrevOrCurrentValue_IsNewFalseHasChangesHasPrevValue_ReturnPrevValue()
    {
        // Arrange
        const string CurrName = "2";
        const string PrevName = "1";
        var domainObject = new PersistentDomainObject { Name = CurrName };
        var change = new ObjectState("Name", CurrName, PrevName, true);
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        this.objectStateService.GetModifiedObjectStates(domainObject).Returns([change]);

        // Act
        var result = this.trackingService.GetPrevOrCurrentValue(domainObject, x => x.Name);

        // Assert
        Assert.Equal(PrevName, result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_RealChangedReferenceProperty_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject { Name = "x" };
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => x.Name);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_RealChangedNullableProperty_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject { ModifiedAt = DateTime.Today };
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => x.ModifiedAt);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_RealChangedValueProperty_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject { CreatedAt = DateTime.Today };
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => x.CreatedAt);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_RealChangedCollectionProperty_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject { Array = [] };
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => x.Array);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_RealNotChangedReferencePropertyNewMode_ResultFalse()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject, GetChangesMode.IgnoreDefaultValues);

        // Act
        var result = changes.HasChange(x => x.Name);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_RealNotChangedReferencePropertyDefaultMode_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => x.Name);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_RealNotChangedNullablePropertyNewMode_ResultFalse()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject, GetChangesMode.IgnoreDefaultValues);

        // Act
        var result = changes.HasChange(x => x.ModifiedAt);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_RealNotChangedNullablePropertyDefaultMode_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => x.ModifiedAt);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_RealNotChangedValuePropertyNewMode_ResultFalse()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject, GetChangesMode.IgnoreDefaultValues);

        // Act
        var result = changes.HasChange(x => x.CreatedAt);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_RealNotChangedValuePropertyDefaultMode_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => x.CreatedAt);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_RealNotChangedArrayPropertyNewMode_ResultFalse()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject, GetChangesMode.IgnoreDefaultValues);

        // Act
        var result = changes.HasChange(x => x.Array);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_RealNotChangedArrayPropertyDefaultMode_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => x.Array);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_ObjectVersionRealChangedValueProperty_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject { CreatedAt = DateTime.Today };
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => (object)x.CreatedAt);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_ObjectVersionRealChangedReferenceProperty_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject { Name = "x" };
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => (object)x.Name);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_ObjectVersionRealChangedNullableProperty_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject { ModifiedAt = DateTime.Today };
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => (object)x.ModifiedAt);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_ObjectVersionRealChangedArrayProperty_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject { Array = [] };
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => (object)x.Array);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_ObjectVersionRealNotChangedReferencePropertyNewMode_ResultFalse()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject, GetChangesMode.IgnoreDefaultValues);

        // Act
        var result = changes.HasChange(x => (object)x.Name);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_ObjectVersionRealNotChangedReferencePropertyDefaultMode_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => (object)x.Name);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_ObjectVersionRealNotChangedNullablePropertyNewMode_ResultFalse()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject, GetChangesMode.IgnoreDefaultValues);

        // Act
        var result = changes.HasChange(x => (object)x.ModifiedAt);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_ObjectVersionRealNotChangedNullablePropertyDefaultMode_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => (object)x.ModifiedAt);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_ObjectVersionRealNotChangedValuePropertyNewMode_ResultFalse()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject, GetChangesMode.IgnoreDefaultValues);

        // Act
        var result = changes.HasChange(x => (object)x.CreatedAt);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_ObjectVersionRealNotChangedValuePropertyDefault_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => (object)x.CreatedAt);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_ObjectVersionRealNotChangedArrayPropertyNewMode_ResultFalse()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject, GetChangesMode.IgnoreDefaultValues);

        // Act
        var result = changes.HasChange(x => (object)x.Array);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Fact]
    public void HasChange_ObjectVersionRealNotChangedArrayPropertyDefaultMode_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => (object)x.Array);

        // Assert
        Assert.True(result);
    }
}
