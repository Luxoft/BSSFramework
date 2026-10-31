namespace Framework.CodeGeneration.GeneratePolicy;

/// <summary>
/// Generation policy
/// </summary>
/// <typeparam name="TIdent"></typeparam>
public interface IGeneratePolicy<in TIdent>
{
    /// <summary>
    /// Usage check
    /// </summary>
    /// <param name="domainType">domain type</param>
    /// <param name="identity">Identifier</param>
    /// <returns></returns>
    bool Used(Type domainType, TIdent identity);
}
