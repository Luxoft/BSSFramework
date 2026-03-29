namespace Framework.CodeGeneration.DTOGenerator.Configuration;

[Flags]
public enum ClientDTORole
{
    Main = 1,

    Strict = 2,

    Update = 4,

    Projection = 8,

    All = Main + Strict + Update + Projection
}
