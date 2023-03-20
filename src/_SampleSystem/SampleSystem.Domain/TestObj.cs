using Framework.Restriction;
using Framework.Validation;

using SampleSystem.Domain.Inline;

namespace SampleSystem.Domain;

public class TestObj : DomainObjectBase
{
    [Required]
    [RestrictionExtension(typeof(RequiredAttribute), CustomError = "aaaa", OperationContext = (int)SampleSystemOperationContext.Save )]
    public Fio FS { get; set; }
}
