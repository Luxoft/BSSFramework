namespace Framework.SecuritySystem;

public record SelfRelativeDomainPathInfo<T>() : SingleRelativeDomainPathInfo<T, T>(v => v);
