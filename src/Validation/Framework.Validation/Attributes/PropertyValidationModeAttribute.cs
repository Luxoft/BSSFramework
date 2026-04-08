namespace Framework.Validation;

/// <summary>
/// Àòðèáóò äëÿ âûáîðî÷íîé âàëèäàöèè ñâîéñòâà
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class PropertyValidationModeAttribute : Attribute
{
    /// <summary>
    /// Ðåæèì âàëèäàöèè ñâîéñòâà (ïî óìîë÷àíèþ âûêëþ÷åíà äëÿ ïðîñòûõ âèðòóàëüíûõ ñâîéñòâ)
    /// </summary>
    public readonly PropertyValidationMode Mode;

    /// <summary>
    /// Ðåæèì âàëèäàöèè âíóòðåííèõ îáúåêòîâ (ïî óìîë÷àíèþ âêëþ÷åíà òîëüêî äëÿ Detail-ñâîéñòâ)
    /// </summary>
    public readonly PropertyValidationMode DeepMode;

    /// <summary>
    /// Êîíñòðóêòîð
    /// </summary>
    /// <param name="mode">Ðåæèì âàëèäàöèè ñâîéñòâà</param>
    /// <param name="deepMode">Ðåæèì âàëèäàöèè âíóòðåííèõ îáúåêòîâ</param>
    public PropertyValidationModeAttribute(PropertyValidationMode mode, PropertyValidationMode deepMode = PropertyValidationMode.Auto)
    {
        this.Mode = mode;
        this.DeepMode = deepMode;
    }

    /// <summary>
    /// Êîíñòðóêòîð
    /// </summary>
    /// <param name="enabled">Ðåæèì âàëèäàöèè ñâîéñòâà</param>
    public PropertyValidationModeAttribute(bool enabled)
        : this(enabled.ToPropertyValidationMode())
    {
    }

    /// <summary>
    /// Ïðîâåðêà íà óêàçàíèå ÿâíîé âàëèäàöèè
    /// </summary>
    /// <param name="value">Çíà÷åíèå</param>
    /// <returns></returns>
    public bool HasValue(bool value) => this.Mode == value.ToPropertyValidationMode();

    /// <summary>
    /// Ïðîâåðêà íà óêàçàíèå ÿâíîé âàëèäàöèè âíóòðåííîãî îáúåêòà
    /// </summary>
    /// <param name="value">Çíà÷åíèå</param>
    /// <returns></returns>
    public bool HasDeepValue(bool value) => this.DeepMode == value.ToPropertyValidationMode();
}