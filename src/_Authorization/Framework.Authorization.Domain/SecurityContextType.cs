namespace Framework.Authorization.Domain;

/// <summary>
/// Description of domain types in whose context rights are granted to the user
/// </summary>
/// <remarks>
/// In code the context is described by the following entities:
/// <seealso cref="SecurityContextType"/>
/// <seealso cref="PermissionRestriction"/>
/// Types in whose context rights are granted to the user are written manually at the SQL level into the database of a specific system
/// </remarks>
public class SecurityContextType : BaseDirectory;
