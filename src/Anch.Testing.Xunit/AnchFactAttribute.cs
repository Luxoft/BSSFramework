using Anch.Testing.Xunit.Engine;

using Xunit;
using Xunit.v3;

namespace Anch.Testing.Xunit;

[XunitTestCaseDiscoverer(typeof(AnchFactDiscoverer))]
[AttributeUsage(AttributeTargets.Method)]
public class AnchFactAttribute : FactAttribute;