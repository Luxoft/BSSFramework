using Framework.Restriction;
using Framework.Validation;

namespace Framework.Configuration.Domain.Models.Filters
{
    public class RegularJobRevisionFilterModel : DomainObjectBase
    {
        [SignValidator(SignType.Positive)]
        public int CountingEntities { get; set; }

        [Required]
        public RegularJob RegularJob { get; set; }
    }
}
