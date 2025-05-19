namespace SPbSTU.OPD.ASAP.Google.Domain.ValueObjects;

public sealed record AssignmentPosition(
    long Id,
    string Title,
    string Cell,
    string QueueSpreadsheetLink,
    string QueueSpreadsheetId);