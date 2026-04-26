using Anch.Core;

namespace Framework.Subscriptions;

public record EmployeeInfo<TEmployee>(PropertyAccessors<TEmployee, string> Email);
