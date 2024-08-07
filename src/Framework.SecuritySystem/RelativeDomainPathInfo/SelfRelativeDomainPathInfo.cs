namespace Framework.SecuritySystem;

public record SelfRelativeDomainPathInfo<T>() : RelativeDomainPathInfo<T, T>(v => v);
