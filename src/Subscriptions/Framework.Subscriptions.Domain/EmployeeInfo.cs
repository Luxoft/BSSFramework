using CommonFramework;

namespace Framework.Subscriptions.Domain;

public record EmployeeInfo<TEmployee>(PropertyAccessors<TEmployee, string> Email);
