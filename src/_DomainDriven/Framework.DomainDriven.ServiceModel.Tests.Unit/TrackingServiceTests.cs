using FluentAssertions;

using Framework.DomainDriven.BLL.Tracking;

using NSubstitute;

using NUnit.Framework;

namespace Framework.DomainDriven.ServiceModel.Tests.Unit;

[TestFixture]
public class TrackingServiceTests
{
    private IObjectStateService objectStateService;

    private TrackingService<PersistentDomainObject> trackingService;

    private IPersistentInfoService persistentInfoService;

    [SetUp]
    public void Initialize()
    {
        this.objectStateService = Substitute.For<IObjectStateService>();
        this.persistentInfoService = Substitute.For<IPersistentInfoService>();
        this.trackingService = new TrackingService<PersistentDomainObject>(this.objectStateService, this.persistentInfoService);
    }

    [Test]
    public void GetPersistentState_IsNewTrue_ReturnNotPersistent()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);

        // Act
        var state = this.trackingService.GetPersistentState(domainObject);

        // Assert
        state.Should().Be(PersistentLifeObjectState.NotPersistent);
    }

    [Test]
    public void GetPersistentState_IsNewFalseIsRemovingFalse_ReturnPersistent()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);

        // Act
        var state = this.trackingService.GetPersistentState(domainObject);

        // Assert
        state.Should().Be(PersistentLifeObjectState.Persistent);
    }

    [Test]
    public void GetPersistentState_IsNewFalseIsRemovingTrue_ReturnMarkAsRemoved()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(true);

        // Act
        var state = this.trackingService.GetPersistentState(domainObject);

        // Assert
        state.Should().Be(PersistentLifeObjectState.MarkAsRemoved);
    }

    [Test]
    public void GetChanges_IsNewFalseIsRemovingFalseNoChanges_ResultShouldBeEmpty()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        this.objectStateService.GetModifiedObjectStates(domainObject).Returns(Enumerable.Empty<ObjectState>());

        // Act
        var result = this.trackingService.GetChanges(domainObject);

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void GetChanges_IsNewFalseIsRemovingFalseOneChange_ResultShouldHaveCountOne()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        var change = new ObjectState("Name", "2", "1", true);
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        this.objectStateService.GetModifiedObjectStates(domainObject).Returns(new[] { change });

        // Act
        var result = this.trackingService.GetChanges(domainObject);

        // Assert
        result.Should().HaveCount(1);
    }

    [Test]
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
        result.Should().HaveCount(objectStates.Length);
    }

    [Test]
    public void GetChanges_IsNewTrue_ResultShouldHaveAllPropertiesChanges()
    {
        // Arrange
        var domainObject = new PersistentDomainObject() { Id = Guid.NewGuid(), CreatedAt = DateTime.Today, Name = "z", Array = new string[0], ModifiedAt = DateTime.Today };
        this.objectStateService.IsNew(domainObject).Returns(true);
        var allProperties = typeof(PersistentDomainObject).GetProperties();

        // Act
        var result = this.trackingService.GetChanges(domainObject);

        // Assert
        result.Should().HaveCount(allProperties.Length);
    }

    [Test]
    public void GetChanges_NewModeIsNewTrueNoPropertiesSet_ResultShouldHaveZeroChanges()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);

        // Act
        var result = this.trackingService.GetChanges(domainObject, GetChangesMode.IgnoreDefaultValues);

        // Assert
        result.Should().HaveCount(0);
    }

    [Test]
    public void GetChanges_DefaultModeIsNewTrueNoPropertiesSet_ResultShouldHaveZeroChanges()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);

        // Act
        var result = this.trackingService.GetChanges(domainObject);

        // Assert
        result.Should().HaveCount(5);
    }

    [Test]
    public void GetChangingState_IsNewTrue_ReturnChanging()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(true);

        // Act
        var result = this.trackingService.GetChangingState(domainObject);

        // Assert
        result.Should().Be(ChangingLifeObjectState.Changing);
    }

    [Test]
    public void GetChangingState_IsNewFalseHasChanges_ReturnChanging()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        var change = new ObjectState("Name", "2", "1", true);
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        this.objectStateService.GetModifiedObjectStates(domainObject).Returns(new[] { change });

        // Act
        var result = this.trackingService.GetChangingState(domainObject);

        // Assert
        result.Should().Be(ChangingLifeObjectState.Changing);
    }

    [Test]
    public void GetChangingState_IsNewFalseNoChanges_ReturnOriginal()
    {
        // Arrange
        var domainObject = new PersistentDomainObject();
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        this.objectStateService.GetModifiedObjectStates(domainObject).Returns(Enumerable.Empty<ObjectState>());

        // Act
        var result = this.trackingService.GetChangingState(domainObject);

        // Assert
        result.Should().Be(ChangingLifeObjectState.Original);
    }

    [Test]
    public void GetPrevValue_IsNewFalseNoChangesNoPrevValue_ReturnDefault()
    {
        // Arrange
        const string CurrName = "2";
        const string DefaultName = "x";
        var domainObject = new PersistentDomainObject { Name = CurrName };
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        this.objectStateService.GetModifiedObjectStates(domainObject).Returns(Enumerable.Empty<ObjectState>());

        // Act
        var result = this.trackingService.GetPrevValue(domainObject, x => x.Name, DefaultName);

        // Assert
        result.Should().Be(DefaultName);
    }

    [Test]
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
        this.objectStateService.GetModifiedObjectStates(domainObject).Returns(new[] { change });

        // Act
        var result = this.trackingService.GetPrevValue(domainObject, x => x.Name, DefaultName);

        // Assert
        result.Should().Be(PrevName);
    }

    [Test]
    public void GetPrevOrCurrentValue_IsNewFalseNoChangesNoPrevValue_ReturnCurrentValue()
    {
        // Arrange
        const string CurrName = "2";
        var domainObject = new PersistentDomainObject { Name = CurrName };
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        this.objectStateService.GetModifiedObjectStates(domainObject).Returns(Enumerable.Empty<ObjectState>());

        // Act
        var result = this.trackingService.GetPrevOrCurrentValue(domainObject, x => x.Name);

        // Assert
        result.Should().Be(CurrName);
    }

    [Test]
    public void GetPrevOrCurrentValue_IsNewFalseHasChangesHasPrevValue_ReturnPrevValue()
    {
        // Arrange
        const string CurrName = "2";
        const string PrevName = "1";
        var domainObject = new PersistentDomainObject { Name = CurrName };
        var change = new ObjectState("Name", CurrName, PrevName, true);
        this.objectStateService.IsNew(domainObject).Returns(false);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        this.objectStateService.GetModifiedObjectStates(domainObject).Returns(new[] { change });

        // Act
        var result = this.trackingService.GetPrevOrCurrentValue(domainObject, x => x.Name);

        // Assert
        result.Should().Be(PrevName);
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeTrue();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeTrue();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeTrue();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
    public void HasChange_RealChangedCollectionProperty_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject { Array = new string[0] };
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => x.Array);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeFalse();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeTrue();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeFalse();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeTrue();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeFalse();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeTrue();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeFalse();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeTrue();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeTrue();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeTrue();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeTrue();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
    public void HasChange_ObjectVersionRealChangedArrayProperty_ResultTrue()
    {
        // Arrange
        var domainObject = new PersistentDomainObject { Array = new string[0] };
        this.objectStateService.IsNew(domainObject).Returns(true);
        this.objectStateService.IsRemoving(domainObject).Returns(false);
        var changes = this.trackingService.GetChanges(domainObject);

        // Act
        var result = changes.HasChange(x => (object)x.Array);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeFalse();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeTrue();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeFalse();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeTrue();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeFalse();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeTrue();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeFalse();
    }

    /// <summary>
    /// IADFRAME-300 TrackingResult.HasChange() и несохранённый объект
    /// </summary>
    [Test]
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
        result.Should().BeTrue();
    }
}
