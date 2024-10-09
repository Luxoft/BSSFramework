namespace Framework.HangfireCore;

public interface IJobNameExtractPolicy
{
    string GetName(Type jobType);

    string GetDisplayName(Type jobType);
}
