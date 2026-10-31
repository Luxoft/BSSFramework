namespace SampleSystem.Domain.Enums;

[Flags]
public enum BusinessUnitOptions
{
    None = 0,
    IsResourcePool = 1,
    TransferNeedApprove = 2,
    AccountStartAllowed = 4,
    ProgramStartAllowed = 8,
    IsSpecialCommission = 16,
    UseExistsFinancialProject = 32,
    CreatePreviousPtsCorrection = 64,
    DoNotPrintNameOnLabel = 128
}
