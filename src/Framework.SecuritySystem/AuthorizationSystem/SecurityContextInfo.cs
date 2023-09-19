namespace Framework.SecuritySystem;

public record SecurityContextInfo<TIdent>(TIdent Id, string Name);
