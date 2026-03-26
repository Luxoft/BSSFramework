using Framework.DomainDriven.Serialization;

namespace Framework.DomainDriven.DTOGenerator.Server;

public static class ServerFileType
{
    public static readonly DTOFileType BaseEventDTO = new DTOFileType(() => BaseEventDTO, DTORole.Event);


    public static readonly DTOFileType SimpleEventDTO = new DTOFileType(() => SimpleEventDTO, DTORole.Event);

    public static readonly DTOFileType RichEventDTO = new DTOFileType(() => RichEventDTO, DTORole.Event);


    public static readonly DTOFileType SimpleIntegrationDTO = new DTOFileType(() => SimpleIntegrationDTO, DTORole.Integration);

    public static readonly DTOFileType RichIntegrationDTO = new DTOFileType(() => RichIntegrationDTO, DTORole.Integration);


    public static readonly FileType LambdaHelper = new FileType(() => LambdaHelper);

    public static readonly FileType ServerDTOMappingServiceInterface = new FileType(() => ServerDTOMappingServiceInterface);

    public static readonly FileType ServerPrimitiveDTOMappingService = new FileType(() => ServerPrimitiveDTOMappingService);

    public static readonly FileType ServerPrimitiveDTOMappingServiceBase = new FileType(() => ServerPrimitiveDTOMappingServiceBase);
}
