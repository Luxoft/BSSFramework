namespace Framework.SecuritySystem.Bss;

public static class BssSecurityOperation
{
    public static SecurityRule SystemIntegration { get; } = new(nameof(SystemIntegration), new Guid("0ba8a6b0-43b9-4f59-90ce-2fcbe37b97c9")) { AdminHasAccess = false, Description = "Can integrate" };

    /// <summary>
    /// Отображение внутренних серверных ошибок клиенту
    /// </summary>
    public static SecurityRule DisplayInternalError { get; } = new(nameof(DisplayInternalError), new Guid("ab8afd01-40d2-48d0-b5f3-a12177b00d0d")) { Description = "Display Internal Error" };
}
