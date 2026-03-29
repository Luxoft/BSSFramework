using System.Reflection;

using Framework.Configuration.Core;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;

/// <summary>
///     Базовый класс процессоров лямбда-выражений подписок.
/// </summary>
public abstract class LambdaProcessor<TBLLContext>
        where TBLLContext : class
{
    private static readonly ComplianceFunctionsTable ComplianceFuncTable = BuildComplianceFuncTable();

    /// <summary>Инициализирует экземпляр конкретного процессора лямбда-выражений.</summary>
    /// <param name="bllContext">Контекст бизнес-логики.</param>
    /// <exception cref="ArgumentNullException">
    ///     Аргумент
    ///     bllContext
    ///     или
    ///     parserFactory равен null.
    /// </exception>
    protected LambdaProcessor(TBLLContext bllContext)
    {
        this.BllContext = bllContext ?? throw new ArgumentNullException(nameof(bllContext));
    }

    /// <summary>Возвращает имя лямбда-выражения.</summary>
    /// <value>Имя лямбда-выражения.</value>
    protected abstract string LambdaName { get; }

    /// <summary>Возвращает контекст бизнес-логики.</summary>
    /// <value>Контекст бизнес-логики.</value>
    protected TBLLContext BllContext { get; }

    /// <summary>
    ///     Определяет, что передаваемые в лямбда-выражение версии доменного объекта, соответствуют ограничениям,
    ///     указанным в описателе лямбда-выражения.
    /// </summary>
    /// <typeparam name="T">Тип доменного объекта</typeparam>
    /// <param name="lambda">Описатель лямбда-выражения.</param>
    /// <param name="versions">Версии доменного объекта.</param>
    /// <returns>
    ///     <c>true</c> если версии доменного объекта соответствуют ограничениям,
    ///     указанным в описателе лямбда-выражения;
    ///     в противном случае <c>false</c>.
    /// </returns>
    protected static bool DomainObjectCompliesLambdaRequiredMode<T>(
            SubscriptionLambda lambda,
            DomainObjectVersions<T> versions)
            where T : class
    {
        var invalidArguments = lambda == null
                               || versions == null
                               || versions.ChangeType == DomainObjectChangeType.Unknown;

        if (invalidArguments)
        {
            return false;
        }

        Func<DomainObjectChangeType, bool> func;
        var success = ComplianceFuncTable.TryGetValue(lambda.ProcessType, out func);

        var result = success && func(versions.ChangeType);
        return result;
    }

    /// <summary>Метод с обработкой исключений для вызова конкретного лямбда-выражения.</summary>
    /// <typeparam name="T1">Тип первого аргумента.</typeparam>
    /// <typeparam name="T2">Тип доменного объекта.</typeparam>
    /// <typeparam name="TResult">Тип результата.</typeparam>
    /// <param name="item">Экземпляр первого аргумента.</param>
    /// <param name="versions">Версии доменного объекта.</param>
    /// <param name="invoke">Делегат, осуществляющий непосредственное исполнение лямбда-выражения.</param>
    /// <returns>Результат исполнения лямбда-выражения.</returns>
    /// <exception cref="SubscriptionServicesException">
    ///     Бросается в случае перехвата любого исключения при вызове делегата,
    ///     осуществляющего непосредственное исполнение лямбда-выражения.
    /// </exception>
    protected TResult TryInvoke<T1, T2, TResult>(
            T1 item,
            DomainObjectVersions<T2> versions,
            Func<T1, DomainObjectVersions<T2>, TResult> invoke)
            where T2 : class
    {
        const string errorMessageTemplate = "{0} lambda for subscription \"{1}\" has failed with message \"{2}\".";

        try
        {
            return invoke(item, versions);
        }
        catch (TargetInvocationException ex)
        {
            var message = string.Format(
                                        errorMessageTemplate,
                                        this.LambdaName,
                                        GetSubscriptionCode(item),
                                        ex.InnerException?.Message ?? ex.Message);

            throw new SubscriptionServicesException(message, ex);
        }
        catch (Exception ex)
        {
            var message = string.Format(
                                        errorMessageTemplate,
                                        this.LambdaName,
                                        GetSubscriptionCode(item),
                                        ex.Message);

            throw new SubscriptionServicesException(message, ex);
        }
    }

    /// <summary>Выполняет приведение переданного значение к указанному типу.</summary>
    /// <typeparam name="TTargetType">Тип, к которому будет приведено переданное значение.</typeparam>
    /// <param name="value">Значение, которое необходимо привести к заданному типу.</param>
    /// <returns>Значение, приведённое к заданному типу.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Переданное значение невозможно привести к заданному типу.
    /// </exception>
    protected TTargetType TryCast<TTargetType>(object value)
    {
        if (value is TTargetType)
        {
            return (TTargetType)value;
        }

        var message = $"Unable cast {this.LambdaName} lambda FuncValue result '{value ?? "null"}' to required type {typeof(TTargetType)}";
        throw new InvalidOperationException(message);
    }

    private static string GetSubscriptionCode(object value)
    {
        return (value as Subscription)?.Code ??
               (value as SubscriptionSecurityItem)?.Subscription.Code ??
               string.Empty;
    }

    private static ComplianceFunctionsTable BuildComplianceFuncTable()
    {
        var create = DomainObjectChangeType.Create;
        var update = DomainObjectChangeType.Update;
        var delete = DomainObjectChangeType.Delete;

        var table = new ComplianceFunctionsTable();

        table[SubscriptionType.Create] = ct => ct == create;
        table[SubscriptionType.Continue] = ct => ct == update;
        table[SubscriptionType.Remove] = ct => ct == delete;
        table[SubscriptionType.CreateOrContinue] = ct => ct == create || ct == update;
        table[SubscriptionType.ContinueOrRemove] = ct => ct == update || ct == delete;
        table[SubscriptionType.All] = t => true;

        return table;
    }

    private class ComplianceFunctionsTable : Dictionary<SubscriptionType, Func<DomainObjectChangeType, bool>>
    {
    }
}
