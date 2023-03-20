using System;

namespace SampleSystem.Domain;

[Flags]
public enum BusinessUnitOptions
{
    None = 0,
    IsResourcePool = 1,
    TransferNeedApprove = 2,
    [Obsolete]
    AccountStartAllowed = 4,
    [Obsolete]
    ProgramStartAllowed = 8,
    IsSpecialCommission = 16,
    UseExistsFinancialProject = 32,
    CreatePreviousPtsCorrection = 64,
    DoNotPrintNameOnLabel = 128
}
