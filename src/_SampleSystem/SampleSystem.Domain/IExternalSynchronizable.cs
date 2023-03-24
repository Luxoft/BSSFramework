using SampleSystem.Domain.Inline;

namespace SampleSystem.Domain;

public interface IExternalSynchronizable
{
    long ExternalId { get; }
}
