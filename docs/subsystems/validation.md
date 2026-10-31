# Валидация

Валидация доменных объектов описывается **атрибутами на модели** и **классовыми валидаторами**. Фреймворк собирает из них карту валидации (`ValidationMap`), генерирует валидаторы в BLL и вызывает их при сохранении.

Проекты: `src/Validation/Framework.Validation.Abstractions` (контракты), `Framework.Validation` (реализация), `Framework.Validation.Domain`, `src/BLL/Framework.BLL.Validation`. Тесты: `Framework.Validation.Tests`.

## Контракты

```csharp
public interface IValidator
{
    ValidationResult GetValidationResult<TSource>(
        TSource source, int operationContext = int.MaxValue, IValidationState? ownerState = null);
}
```

`ValidationResult` содержит успех или набор ошибок. `ValidationResult.Success` — успешный результат, `ValidationResult.CreateError(message)` — ошибка.

`operationContext` — числовой контекст операции, который передаётся во все контексты валидации (`IClassValidationContext`, `IPropertyValidationContext`). По умолчанию `int.MaxValue`.

## Атрибуты свойств

Ограничения из `Framework.Restriction` участвуют и в валидации, и в схеме БД:

| Атрибут | Эффект |
|---|---|
| `[Required]` | Значение обязательно (режим `RequiredMode`) |
| `[MaxLength(n)]` | Максимальная длина строки |
| `[UniqueGroup]`, `[UniqueElement]` | Уникальность по группе полей или элементу |

Атрибуты валидации (`Framework.Validation.Attributes`):

| Атрибут | Проверка |
|---|---|
| `[RequiredValidator]`, `[RequiredGroupValidator]` | Обязательность поля или хотя бы одного из группы |
| `[MaxLengthValidator]` | Длина строки |
| `[EnglishAlphabetValidator]`, `[NumberAlphabetValidator]` | Допустимый алфавит |
| `[IntValueValidator]`, `[Int64ValueValidator]`, `[SignValidator]` | Диапазон и знак числа |
| `[AnyElementsValidator]`, `[UniqueCollectionValidator]` | Непустая коллекция, уникальность элементов |
| `[ExpandValidation]` | Рекурсивная проверка вложенного объекта |
| `[PropertyValidationMode(...)]` | Режим для свойства: `Disabled`, `Auto`, `Enabled` |

`PropertyValidationMode` управляет тем, проверяется ли свойство. `Auto` выбирает режим по внутренним политикам фреймворка, `Enabled` и `Disabled` принудительны:

```csharp
[PropertyValidationMode(true)]
public virtual DateTime ValidateVirtualProp { get; set; }

[PropertyValidationMode(false)]
public virtual string CellPhone { get; set; }
```

Атрибут применяется к свойствам. Конструктор принимает `bool` либо `PropertyValidationMode mode` и необязательный `deepMode`. Режим `mode` относится к самому свойству (по умолчанию проверка отключена для простых виртуальных свойств). `deepMode` относится к вложенным объектам (по умолчанию включена только для Detail-свойств).

## Глобальные валидаторы для всех типов

Классовые атрибуты можно добавить сразу всем типам через `ExtendedAttributeSource` в окружении генерации (`ServerGenerationEnvironment.GetExtendedAttributeSources`):

```csharp
.Add<DomainObjectBase>(tb => tb.AddAttribute<AvailableDecimalValidatorAttribute>()
                               .AddAttribute<AvailablePeriodValidatorAttribute>()
                               .AddAttribute<AvailableDateTimeValidatorAttribute>()
                               .AddAttribute<DefaultStringMaxLengthValidatorAttribute>())
```

| Атрибут | Правило |
|---|---|
| `DefaultStringMaxLengthValidatorAttribute` | Ограничение длины для всех строк без явного `[MaxLength]` |
| `AvailableDecimalValidatorAttribute` | Проверка допустимых значений `decimal` |
| `AvailableDateTimeValidatorAttribute` | Проверка допустимых значений дат |
| `AvailablePeriodValidatorAttribute` | Проверка допустимых значений периодов |

Точные границы допустимых значений смотрите в `Framework.Validation/Validators/DynamicClass/Available`.

## Собственный классовый валидатор

1. Реализуйте `IClassValidator<T>`:

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

2. Создайте атрибут-фабрику:

```csharp
public sealed class EmployeeValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator() => new EmployeeValidator();
}
```

3. Подключите атрибут к типу. Можно поставить его прямо на класс, как в примере из SampleSystem, через расширенные атрибуты в окружении генерации (`.Add<Employee>(b => b.AddAttribute<EmployeeValidatorAttribute>())`).
4. Запустите генерацию (см. [codegeneration.md](../codegeneration.md)): валидатор попадёт в сгенерированную `ValidationMap` (`*.ValidationMap.Generated.cs`, `*.Validator.Generated.cs` в `SampleSystem.BLL/_Generated`).

Код валидатора лежит в отдельном проекте (`SampleSystem.Validation`), чтобы его могли использовать и генераторы, и рантайм.

## Тесты

- Юнит-тесты фреймворка: `src/Validation/Framework.Validation.Tests`.
- Интеграционные: `ValidationTests`, `FixedPropertyValidatorTests`, `UniqueGroupTests` в `SampleSystem.IntegrationTests`.
