using System.Linq.Expressions;

using SampleSystem.Domain;

namespace SampleSystem.Security.Services;

public record ToEmployeePathInfo<TDomainObject>(Expression<Func<TDomainObject, Employee>> Path);
