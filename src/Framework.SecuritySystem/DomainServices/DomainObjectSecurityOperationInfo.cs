#nullable enable

namespace Framework.SecuritySystem;

public record DomainObjectSecurityOperationInfo(Type DomainType, SecurityOperation? ViewOperation, SecurityOperation? EditOperation);
