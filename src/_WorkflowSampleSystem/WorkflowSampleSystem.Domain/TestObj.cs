using Framework.Restriction;
using Framework.Validation;

using WorkflowSampleSystem.Domain.Inline;

namespace WorkflowSampleSystem.Domain
{
    public class TestObj : DomainObjectBase
    {
        [Required]
        [RestrictionExtension(typeof(RequiredAttribute), CustomError = "aaaa", OperationContext = (int)WorkflowSampleSystemOperationContext.Save )]
        public Fio FS { get; set; }
    }
}
