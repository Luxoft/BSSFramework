using Framework.BLL.Domain.DTO;

namespace Framework.BLL.Domain.ServiceRole;

/// <summary>
/// Base attribute for viewing objects through the facade layer (Visual, Simple, Full, Rich, Projection)
/// </summary>
public abstract class BLLViewRoleBaseAttribute : BLLServiceRoleAttribute, IMaxFetchContainer
{
    /// <summary>
    /// Maximum database fetch level
    /// </summary>
    protected abstract ViewDTOType BaseMaxFetch { get; }

    ViewDTOType IMaxFetchContainer.MaxFetch => this.BaseMaxFetch;
}
