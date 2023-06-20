namespace Framework.HangfireCore.JobServices;

public class HangfireCredentialSettings : IHangfireCredentialSettings
{
    public string RunAs { get; set; } = "HangfireJob";
}
