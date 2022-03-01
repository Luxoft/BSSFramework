using WorkflowSampleSystem.Domain.Inline;

namespace WorkflowSampleSystem.Domain
{
    public interface IExternalSynchronizable
    {
        long ExternalId { get; }
    }
}
