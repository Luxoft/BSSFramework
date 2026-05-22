using Microsoft.Extensions.DependencyInjection;

namespace Anch.Testing;

public class ParallelizationSettings([FromKeyedServices] IEnumerable<AllowParallelizationConstraint> constraints) : IParallelizationSettings
{
    public bool AllowParallelization { get; } = constraints.All(c => c.Value);
}