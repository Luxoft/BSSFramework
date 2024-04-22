namespace Framework.Configuration.Domain;

public interface ILegacyForceEventSystem
{
    void ForceEvent(DomainTypeEventModel domainTypeEventModel);
}
