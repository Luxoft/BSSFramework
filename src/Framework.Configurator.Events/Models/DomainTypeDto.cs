using Framework.Events;

namespace Framework.Configurator.Models;

public class DomainTypeDto
{
    public string Name { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public List<EventOperation> Operations { get; set; } = null!;
}
