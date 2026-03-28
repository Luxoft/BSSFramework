namespace Framework.Infrastructure.Hangfire;

public interface IJobNameExtractPolicy
{
    string GetName(Type jobType);

    string GetDisplayName(Type jobType);
}
