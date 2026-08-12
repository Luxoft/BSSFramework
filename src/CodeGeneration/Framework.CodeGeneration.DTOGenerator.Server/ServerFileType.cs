using Framework.BLL.Domain.Serialization;
using Framework.CodeGeneration.DTOGenerator.FileTypes;

namespace Framework.CodeGeneration.DTOGenerator.Server;

public static class ServerFileType
{
    public static DTOFileType BaseEventDTO { get; } = new(nameof(BaseEventDTO), DTORole.Event);


    public static DTOFileType SimpleEventDTO { get; } = new(nameof(SimpleEventDTO), DTORole.Event);

    public static DTOFileType RichEventDTO { get; } = new(nameof(RichEventDTO), DTORole.Event);


    public static DTOFileType SimpleIntegrationDTO { get; } = new(nameof(SimpleIntegrationDTO), DTORole.Integration);

    public static DTOFileType RichIntegrationDTO { get; } = new(nameof(RichIntegrationDTO), DTORole.Integration);


    public static BaseFileType LambdaHelper { get; } = new(nameof(LambdaHelper));

    public static BaseFileType ServerDTOMappingServiceInterface { get; } = new(nameof(ServerDTOMappingServiceInterface));

    public static BaseFileType ServerPrimitiveDTOMappingService { get; } = new(nameof(ServerPrimitiveDTOMappingService));

    public static BaseFileType ServerPrimitiveDTOMappingServiceBase { get; } = new(nameof(ServerPrimitiveDTOMappingServiceBase));
}
