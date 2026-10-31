# Legacy-код

В репозитории осталась часть старого API. Он нужен существующим приложениям, но **в новом коде его использовать не нужно**. Здесь собран перечень, чтобы его было легко распознать.

> Планов удаления в репозитории нигде не зафиксировано. Если они появятся, укажите их в этом документе и в `[Obsolete]`-сообщениях.

## Как распознать

- Проекты и папки с `Legacy` в названии.
- Методы расширений `AddLegacy…` и `AddSupportLegacyServices` в DI-композиции. В `SampleSystemGeneralDependencyInjectionExtensions` они сгруппированы под комментарием `// Legacy`.
- Проекции `*.LegacyProjections`.

## Проекты

| Проект | Содержимое |
|---|---|
| `Database/Framework.Database.Legacy.Abstractions` | `IDAL`, `IAuditDAL`: старый интерфейс доступа к данным |
| `Database/Framework.Database.Legacy.Domain` | Модели ревизий аудита (`DomainObjectRevision`, `DomainObjectPropertyRevisions`, `ObjectModificationInfo`, …) |
| `Database/Framework.Database.NHibernate.Legacy` | `NHibDal`, `NHibObjectStatesService`, `AddLegacyNHibernateSettings` |
| `Database/Framework.Database.EntityFramework.Legacy` | `EfDal`, `EfObjectStatesService`, `AddLegacyEntityFrameworkSettings` |
| `Infrastructure/Framework.Infrastructure.Legacy` | Базовые контроллеры `ApiControllerBase<…>` и `ContextEvaluator` |
| `Infrastructure/Framework.Infrastructure.Legacy.Default` | Реализации по умолчанию: `SubscriptionDALListener`, `FixDomainObjectEventRevisionNumberDALListener`, настройка target-систем (`ITargetSystemSetup`), `EventDTOMessageSenderBase` |
| `AutomationCore/Framework.AutomationCore.Legacy` | Старая база интеграционных тестов (`IntegrationTestBase`, расширения `ContextEvaluator`) |
| `_SampleSystem/SampleSystem.Domain.LegacyProjections` | Сгенерированные «старые» проекции (генерируются `LegacyProjectionGeneratorConfiguration` без `DependencySecurity`) |

## Что относится к legacy в DI-композиции

В `AddGeneralDependencyInjection` под `// Legacy`:

| Вызов | Назначение |
|---|---|
| `AddSubscriptions`, `AddSubscriptionManagers` | Подписки на события и менеджеры подписок |
| `AddLegacyGenericServices`, `AddLegacyDefaultGenericServices` | Старые обобщённые сервисы |
| `AddContextEvaluators` | `IContextEvaluator` |
| `AddLegacyNotification` | Подключение отправки уведомлений |
| `AddConfigurationSystemConstants`, `AddConfigurationTargetSystems` | Системные константы и target-системы подсистемы Configuration |
| `AddBLLSystem` | Регистрация BLL-контекста |
| `AddSupportLegacyServices` | Маппинг DTO (`SetDTOMapping`) |

Часть этих вызовов остаётся актуальной (например, подписки: см. [notification.md](subsystems/notification.md)). Название «Legacy» там означает, что механизм живёт в старой инфраструктуре, а не что он не нужен.

## Правила для разработчика

1. Не добавляйте в новые проекты ссылки на `*.Legacy*`, если без этого можно обойтись.
2. При правке legacy-проектов делайте минимальные изменения и сохраняйте совместимость.
3. Новые интеграционные тесты пишите на `SampleSystem.IntegrationTests` + `Framework.AutomationCore`, а не на `Framework.AutomationCore.Legacy`.
4. Если вы нашли, чем заменить конкретный legacy-тип, пометьте его `[Obsolete("Use … instead")]` и опишите замену в этой таблице.
