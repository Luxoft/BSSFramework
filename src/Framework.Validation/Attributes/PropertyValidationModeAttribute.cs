using System;

namespace Framework.Validation
{
    /// <summary>
    /// ������� ��� ���������� ��������� ��������
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyValidationModeAttribute : Attribute
    {
        /// <summary>
        /// ����� ��������� �������� (�� ��������� ��������� ��� ������� ����������� �������)
        /// </summary>
        public readonly PropertyValidationMode Mode;

        /// <summary>
        /// ����� ��������� ���������� �������� (�� ��������� �������� ������ ��� Detail-�������)
        /// </summary>
        public readonly PropertyValidationMode DeepMode;

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="mode">����� ��������� ��������</param>
        /// <param name="deepMode">����� ��������� ���������� ��������</param>
        public PropertyValidationModeAttribute(PropertyValidationMode mode, PropertyValidationMode deepMode = PropertyValidationMode.Auto)
        {
            this.Mode = mode;
            this.DeepMode = deepMode;
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="enabled">����� ��������� ��������</param>
        public PropertyValidationModeAttribute(bool enabled)
            : this(enabled.ToPropertyValidationMode())
        {
        }

        /// <summary>
        /// �������� �� �������� ����� ���������
        /// </summary>
        /// <param name="value">��������</param>
        /// <returns></returns>
        public bool HasValue(bool value)
        {
            return this.Mode == value.ToPropertyValidationMode();
        }

        /// <summary>
        /// �������� �� �������� ����� ��������� ����������� �������
        /// </summary>
        /// <param name="value">��������</param>
        /// <returns></returns>
        public bool HasDeepValue(bool value)
        {
            return this.DeepMode == value.ToPropertyValidationMode();
        }
    }
}