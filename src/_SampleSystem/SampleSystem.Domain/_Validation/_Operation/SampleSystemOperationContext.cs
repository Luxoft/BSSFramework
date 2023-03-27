namespace SampleSystem.Domain;

/// <summary>
/// Перечень действий, который можно делать c объектом
/// </summary>
[Flags]
public enum SampleSystemOperationContext : int
{
    Request = 1,
    Save = 2,
    Confirm = 4,
    Register = 8,
    RequestProgramStart = 16,
    AdmProjectStartRequest = 32,
    FinRequest = 64,
    RequestAccountStart = 128,
    Approve = 256,

    AllWithoutSave = Request
                     + Confirm
                     + Register
                     + RequestProgramStart
                     + AdmProjectStartRequest
                     + FinRequest
                     + RequestAccountStart
                     + Approve,

    All = Request
          + Save
          + Confirm
          + Register
          + RequestProgramStart
          + AdmProjectStartRequest
          + FinRequest
          + RequestAccountStart
          + Approve,
}
