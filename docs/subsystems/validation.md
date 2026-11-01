# Validation

Validation of domain objects is described by **attributes on the model** and **class validators**. The framework builds a validation map (`ValidationMap`) from them, generates validators in the BLL and calls them on save.

Projects: `src/Validation/Framework.Validation.Abstractions` (contracts), `Framework.Validation` (implementation), `Framework.Validation.Domain`, `src/BLL/Framework.BLL.Validation`. Tests: `Framework.Validation.Tests`.

## Contracts

```csharp
public interface IValidator
{
    ValidationResult GetValidationResult<TSource>(
        TSource source, int operationContext = int.MaxValue, IValidationState? ownerState = null);
}
```

`ValidationResult` holds success or a set of errors. `ValidationResult.Success` is a successful result, `ValidationResult.CreateError(message)` is an error.

`operationContext` is a numeric operation context that is passed to all validation contexts (`IClassValidationContext`, `IPropertyValidationContext`). Defaults to `int.MaxValue`.

## Property attributes

Restrictions from `Framework.Restriction` take part both in validation and in the DB schema:

| Attribute | Effect |
|---|---|
| `[Required]` | The value is mandatory (mode `RequiredMode`) |
| `[MaxLength(n)]` | Maximum string length |
| `[UniqueGroup]`, `[UniqueElement]` | Uniqueness by a group of fields or an element |

Validation attributes (`Framework.Validation.Attributes`):

| Attribute | Check |
|---|---|
| `[RequiredValidator]`, `[RequiredGroupValidator]` | A field, or at least one field of a group, is required |
| `[MaxLengthValidator]` | String length |
| `[EnglishAlphabetValidator]`, `[NumberAlphabetValidator]` | Allowed alphabet |
| `[IntValueValidator]`, `[Int64ValueValidator]`, `[SignValidator]` | Number range and sign |
| `[AnyElementsValidator]`, `[UniqueCollectionValidator]` | Non-empty collection, uniqueness of elements |
| `[ExpandValidation]` | Recursive check of a nested object |
| `[PropertyValidationMode(...)]` | Mode for a property: `Disabled`, `Auto`, `Enabled` |

`PropertyValidationMode` controls whether a property is checked. `Auto` picks the mode according to the framework's internal policies, `Enabled` and `Disabled` are forced:

```csharp
[PropertyValidationMode(true)]
public virtual DateTime ValidateVirtualProp { get; set; }

[PropertyValidationMode(false)]
public virtual string CellPhone { get; set; }
```

The attribute applies to properties. The constructor takes a `bool`, or a `PropertyValidationMode mode` and an optional `deepMode`. `mode` refers to the property itself (by default checking is disabled for simple virtual properties). `deepMode` refers to nested objects (by default enabled only for Detail properties).

## Global validators for all types

Class attributes can be added to all types at once via `ExtendedAttributeSource` in the generation environment (`ServerGenerationEnvironment.GetExtendedAttributeSources`):

```csharp
.Add<DomainObjectBase>(tb => tb.AddAttribute<AvailableDecimalValidatorAttribute>()
                               .AddAttribute<AvailablePeriodValidatorAttribute>()
                               .AddAttribute<AvailableDateTimeValidatorAttribute>()
                               .AddAttribute<DefaultStringMaxLengthValidatorAttribute>())
```

| Attribute | Rule |
|---|---|
| `DefaultStringMaxLengthValidatorAttribute` | Length limit for all strings without an explicit `[MaxLength]` |
| `AvailableDecimalValidatorAttribute` | Check of allowed `decimal` values |
| `AvailableDateTimeValidatorAttribute` | Check of allowed date values |
| `AvailablePeriodValidatorAttribute` | Check of allowed period values |

For the exact bounds of allowed values see `Framework.Validation/Validators/DynamicClass/Available`.

## Custom class validator

1. Implement `IClassValidator<T>`:

```csharp
public sealed class EmployeeValidator : IClassValidator<Employee>
{
    public const int ForbiddenPin = 1234;

    public ValidationResult GetValidationResult(IClassValidationContext<Employee> validationContext)
    {
        var source = validationContext.Source;

        if (source.Pin == ForbiddenPin)
        {
            return ValidationResult.CreateError($"Employee Pin could not be set as '{ForbiddenPin}'");
        }

        return ValidationResult.Success;
    }
}
```

2. Create a factory attribute:

```csharp
public sealed class EmployeeValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator() => new EmployeeValidator();
}
```

3. Attach the attribute to the type. You can put it directly on the class, as in the SampleSystem example, via extended attributes in the generation environment (`.Add<Employee>(b => b.AddAttribute<EmployeeValidatorAttribute>())`).
4. Run generation (see [codegeneration.md](../codegeneration.md)): the validator will end up in the generated `ValidationMap` (`*.ValidationMap.Generated.cs`, `*.Validator.Generated.cs` in `SampleSystem.BLL/_Generated`).

The validator code lives in a separate project (`SampleSystem.Validation`) so that both generators and the runtime can use it.

## Tests

- Framework unit tests: `src/Validation/Framework.Validation.Tests`.
- Integration: `ValidationTests`, `FixedPropertyValidatorTests`, `UniqueGroupTests` in `SampleSystem.IntegrationTests`.
