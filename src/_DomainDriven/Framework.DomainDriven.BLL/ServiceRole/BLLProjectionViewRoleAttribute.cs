using Framework.Transfering;

namespace Framework.DomainDriven.BLL
{
    /// <summary>
    /// ������� �������� ��������� ��������� � BLL ���� ��� ��������
    /// </summary>
    public class BLLProjectionViewRoleAttribute : BLLViewRoleBaseAttribute
    {
        /// <summary>
        /// ������������ ������� �������� �� ����
        /// </summary>
        public ViewDTOType MaxFetch { get; set; } = ViewDTOType.ProjectionDTO;

        /// <summary>
        /// ������������ ������� �������� �� ����
        /// </summary>
        protected override ViewDTOType BaseMaxFetch => this.MaxFetch;
    }
}