using Framework.Transfering;

namespace Framework.DomainDriven.BLL
{
    /// <summary>
    /// Àòðèáóò óêàçàíèÿ ãåíåðàöèè ôàñàäíîãî è BLL ñëî¸â äëÿ ïðîåêöèé
    /// </summary>
    public class BLLProjectionViewRoleAttribute : BLLViewRoleBaseAttribute
    {
        /// <summary>
        /// Ìàêñèìàëüíûé óðîâåíü âûãðóçêè èç áàçû
        /// </summary>
        public ViewDTOType MaxFetch { get; set; } = ViewDTOType.ProjectionDTO;

        /// <summary>
        /// Ìàêñèìàëüíûé óðîâåíü âûãðóçêè èç áàçû
        /// </summary>
        protected override ViewDTOType BaseMaxFetch => this.MaxFetch;
    }
}
